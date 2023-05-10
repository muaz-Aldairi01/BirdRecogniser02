namespace BirdRecogniser02.ML.DataModels
{
    public class ImagePredictedLabelWithProbability
    {
        public string? ImageId;

        public string? PredictedLabel { get; set; }
        public float Probability { get; set; }
        public string? GeneralInfo { get; set; }

        public long PredictionExecutionTime;
    }
}
