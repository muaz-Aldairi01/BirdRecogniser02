using Microsoft.AspNetCore.Mvc;


namespace BirdRecogniser02.Controllers
    {
        public class ResultController : Controller
        {
        [HttpPost]
        public IActionResult Index(string prediction0, string probability0, string prediction1, string probability1, string prediction2, string probability2, string croppedImageDataUrl)
        {
            ViewData["Prediction0"] = prediction0;
            ViewData["Probability0"] = probability0;
            ViewData["Prediction1"] = prediction1;
            ViewData["Probability1"] = probability1;
            ViewData["Prediction2"] = prediction2;
            ViewData["Probability2"] = probability2;
            ViewData["CroppedImageDataUrl"] = croppedImageDataUrl;

            return View();
        }


    }
}

