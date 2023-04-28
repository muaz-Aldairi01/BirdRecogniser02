using BirdRecogniser02.ImageHelpers;
using BirdRecogniser02.ML.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BirdRecogniser02.Controllers
{
    public class RecogniseDemoController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
