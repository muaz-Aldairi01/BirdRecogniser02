using BirdRecogniser02.ImageHelpers;
using BirdRecogniser02.ML.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

            // Predict the image's label (The one with highest probability).
            //ImagePredictedLabelWithProbability imageBestLabelPrediction
            //                    = FindBestLabelWithProbability(imageLabelPredictions, imageInputData);

            //return Ok(imageBestLabelPrediction);

            //============================================================================
            var imageBest3LabelsPrediction
                                  = FindBest3LabelsWithProbability(imageLabelPredictions, imageInputData);
            return Ok(imageBest3LabelsPrediction);
            //==============================================================================
        }

        //===========================================================================
        private List<ImagePredictedLabelWithProbability> FindBest3LabelsWithProbability(ImageLabelPrediction imageLabelPredictions, ImageInputData imageInputData)
        {
            // Read TF model's labels (labels.txt) to classify the image across those labels.
            var labels = ReadLabels(_labelsFilePath);

            float[] probabilities = imageLabelPredictions.PredictedLabels;


            var imageBest3LabelsPrediction = new List<ImagePredictedLabelWithProbability>(3);
            for (int i = 0; i < 3; i++)
            {
                (string predictedLabel, float probability) = GetBestLabel(labels, probabilities);
                if (probability < 0.01)
                    break;
                var imageLabelPrediction = new ImagePredictedLabelWithProbability
                {
                    ImageId = imageInputData.GetHashCode().ToString(),
                    PredictedLabel = predictedLabel,
                    Probability = probability
                };
                imageBest3LabelsPrediction.Add(imageLabelPrediction);
                // Set the probability of the predicted label to 0 so that we can get the next best label in the next iteration.
                probabilities[Array.IndexOf(labels, predictedLabel)] = 0;
            }

            return imageBest3LabelsPrediction;
        }

        //===========================================================================

        //private ImagePredictedLabelWithProbability FindBestLabelWithProbability(ImageLabelPrediction imageLabelPredictions, ImageInputData imageInputData)
        //    {
        //        // Read TF model's labels (labels.txt) to classify the image across those labels.
        //        var labels = ReadLabels(_labelsFilePath);

        //        float[] probabilities = imageLabelPredictions.PredictedLabels;

        //        // Set a single label as predicted or even none if probabilities were lower than 70%.
        //        var imageBestLabelPrediction = new ImagePredictedLabelWithProbability()
        //        {
        //            ImageId = imageInputData.GetHashCode().ToString(), //This ID is not really needed, it could come from the application itself, etc.
        //        };

        //        (imageBestLabelPrediction.PredictedLabel, imageBestLabelPrediction.Probability) = GetBestLabel(labels, probabilities);

        //        return imageBestLabelPrediction;
        //    }



        private (string, float) GetBestLabel(string[] labels, float[] probs)
        {
            var max = probs.Max();
            var index = probs.AsSpan().IndexOf(max);

            return (labels[index], max);
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
