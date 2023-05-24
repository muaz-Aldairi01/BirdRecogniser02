using BirdRecogniser02.ImageHelpers;
using BirdRecogniser02.ML.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using System.Drawing;

namespace BirdRecogniser02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecogniseAPIController : ControllerBase
    {

        public IConfiguration Configuration { get; }
        private readonly PredictionEnginePool<ImageInputData, ImageLabelPrediction> _predictionEnginePool;
        private readonly ILogger<RecogniseAPIController> _logger;
        private readonly string _labelsFilePath;

        public RecogniseAPIController(PredictionEnginePool<ImageInputData, ImageLabelPrediction> predictionEnginePool, IConfiguration configuration, ILogger<RecogniseAPIController> logger)
        {
            _predictionEnginePool = predictionEnginePool;

            Configuration = configuration;
            _labelsFilePath = Path.Combine(configuration["MLModel:LabelsFilePath"]);

            _logger = logger;

        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("classifyimage")]
        [AllowAnonymous]
        public async Task<IActionResult> ClassifyImage(IFormFile imageFile)
        {
            if (imageFile.Length == 0)
                return BadRequest();

            var imageMemoryStream = new MemoryStream();
            await imageFile.CopyToAsync(imageMemoryStream);

            // Check that the image is valid.
            byte[] imageData = imageMemoryStream.ToArray();
            if (!imageData.IsValidImage())
                return StatusCode(StatusCodes.Status415UnsupportedMediaType);

            // Convert to Image.
            Image image = Image.FromStream(imageMemoryStream);

            // Convert to Bitmap.
            Bitmap bitmapImage = (Bitmap)image;

            _logger.LogInformation("Start processing image...");

            // Measure execution time.
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Set the specific image data into the ImageInputData type used in the DataView.
            var imageInputData = new ImageInputData { Image = bitmapImage };

            // Predict code for provided image.
            ImageLabelPrediction imageLabelPredictions = _predictionEnginePool.Predict(imageInputData);

            // Stop measuring time.
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            _logger.LogInformation($"Image processed in {elapsedMs} miliseconds");

            // Predict the images' labels (Three with highest probability).
            var imageBest3LabelsPrediction
                                  = FindBest3LabelsWithProbability(imageLabelPredictions, imageInputData);
            return Ok(imageBest3LabelsPrediction);
            
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("birdsinfo")]
        [AllowAnonymous]

        public async Task<IActionResult> GetBirdsInfo(string birdNames)
        {
            if (birdNames.Length == 0)
                return BadRequest();

            var names = birdNames.ToUpper().Split(',');
            List<BirdInformation> information = new List<BirdInformation>();

            List<string> columnValues = new List<string>();

            using (var reader = new StreamReader("wwwroot/general/general.csv"))
            {

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var name = values[0].ToUpper();
                    if (names.Contains(name))
                    {
                        BirdInformation information1 = new BirdInformation();
                        information1.PredictedLabel = name;
                        information1.GeneralInfo = values[1];
                        information.Add(information1);
                    }
                }
            }


            return Ok(information.ToArray());
            //==============================================================================
        }

        //===========================================================================

        private List<ImagePredictedLabelWithProbability> FindBest3LabelsWithProbability(ImageLabelPrediction imageLabelPredictions, ImageInputData imageInputData)
        {
            // Read TF model's labels (labels.txt) to classify the image across those labels.
            var labels = ReadLabels(_labelsFilePath);

            float[] probabilities = imageLabelPredictions.PredictedLabels;

            
            List<string> columnValues = new List<string>();

            using (var reader = new StreamReader("wwwroot/general/general.csv"))
            {
               

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    columnValues.Add(values[1]); 
                }    
            }
            string[] generalInfos = columnValues.ToArray();

            var imageBest3LabelsPrediction = new List<ImagePredictedLabelWithProbability>(3);
            for (int i = 0; i < 3; i++)
            {
                (string predictedLabel, float probability, string generalInfo) = GetBestLabel(labels, probabilities,generalInfos);
                if (probability < 0.01)
                    break;
                var imageLabelPrediction = new ImagePredictedLabelWithProbability
                {
                    ImageId = imageInputData.GetHashCode().ToString(),
                    PredictedLabel = predictedLabel,
                    Probability = probability,
                    GeneralInfo = generalInfo
                };
                imageBest3LabelsPrediction.Add(imageLabelPrediction);
                // Set the probability of the predicted label to 0 so that we can get the next best label in the next iteration.
                probabilities[Array.IndexOf(labels, predictedLabel)] = 0;
            }

            return imageBest3LabelsPrediction;
        }

        private (string, float,string) GetBestLabel(string[] labels, float[] probs, string[] infos)
        {
            var max = probs.Max();
            var index = probs.AsSpan().IndexOf(max);

            return (labels[index], max, infos[index+1]);
        }

        private string[] ReadLabels(string labelsLocation)
        {
            return System.IO.File.ReadAllLines(labelsLocation);
        }

        public static string GetAbsolutePath(string relativePath)
        {
            var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);
            return fullPath;
        }

        // GET api/ImageClassification
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "ACK Heart beat 1", "ACK Heart beat 2" };
        }

    }
}
