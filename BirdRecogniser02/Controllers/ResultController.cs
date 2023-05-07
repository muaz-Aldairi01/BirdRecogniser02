using Microsoft.AspNetCore.Mvc;


namespace BirdRecogniser02.Controllers
    {
        public class ResultController : Controller
        {
        public IActionResult Index(string prediction, string probability, string croppedImageDataUrl)
        {
            ViewData["Prediction"] = prediction;
            ViewData["Probability"] = probability;
            ViewData["CroppedImageDataUrl"] = croppedImageDataUrl;

            return View();
        }

    }
}

