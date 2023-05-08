using Microsoft.ML.Data;

namespace BirdRecogniser02.ML.DataModels
{
    public class ImageLabelPrediction
    {
        [ColumnName("Identity")]
        public float[] PredictedLabels;
    }
}
