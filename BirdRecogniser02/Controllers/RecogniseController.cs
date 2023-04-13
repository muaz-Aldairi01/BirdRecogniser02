using Microsoft.AspNetCore.Mvc;

namespace BirdRecogniser02.Controllers
{
    public class RecogniseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
