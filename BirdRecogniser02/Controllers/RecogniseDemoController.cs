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
using Microsoft.AspNetCore.Authorization;

namespace BirdRecogniser02.Controllers
{
    [AllowAnonymous]
    public class RecogniseDemoController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
