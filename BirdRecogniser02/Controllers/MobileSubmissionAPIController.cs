using BirdRecogniser02.Data;
using BirdRecogniser02.ML.DataModels;
using BirdRecogniser02.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BirdRecogniser02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileSubmissionAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MobileSubmissionAPIController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment; 
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("submitBird")]
        [AllowAnonymous]

        public async Task<IActionResult> GetBirdsInfo(IFormCollection birdInformation)
        {
            if (birdInformation != null)
            {
                var image = birdInformation.Files["birdImage"];
                var birdName = birdInformation["birdName"].ToString();
                var birdInfo = birdInformation["birdInfo"].ToString();
                
                if (birdName != null && image != null)
                {
                    Submission submission = new Submission();
                    submission.BirdName = birdName;
                    submission.BirdInformation = birdInfo;
                    submission.Status = SubmissionStatus.Submitted;

                    //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = image.FileName;
                    string extension = Path.GetExtension(fileName);
                    submission.FileName =  image.Name + DateTime.Now.ToString("MMddyyyyhhmmssffftt") + extension;
                    string path = Path.Combine(wwwRootPath + "/Photos/", submission.FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    _context.Submission.Update(submission);
                    await _context.SaveChangesAsync();

                    return Ok("Success");
                }
            }

            return BadRequest();
        }
    }
}
