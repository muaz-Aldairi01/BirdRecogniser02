using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BirdRecogniser02.Controllers
    {
        public class ResultController : Controller
        {
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(string prediction0, string probability0, string prediction1, string probability1, string prediction2, string probability2, string generalInf0, string generalInf1, string generalInf2,string croppedImageDataUrl)
        {
            ViewData["Prediction0"] = prediction0;
            ViewData["Probability0"] = probability0;
            ViewData["GeneralInf0"] = generalInf0;

            ViewData["Prediction1"] = prediction1;
            ViewData["Probability1"] = probability1;
            ViewData["GeneralInf1"] = generalInf1;

            ViewData["Prediction2"] = prediction2;
            ViewData["Probability2"] = probability2;
            ViewData["GeneralInf2"] = generalInf2;
            ViewData["CroppedImageDataUrl"] = croppedImageDataUrl;

            return View();
        }


    }
}

