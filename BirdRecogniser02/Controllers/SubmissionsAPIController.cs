using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BirdRecogniser02.Data;
using BirdRecogniser02.Models;
using Microsoft.AspNetCore.Authorization;

namespace BirdRecogniser02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SubmissionsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SubmissionsAPIController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/SubmissionsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmission()
        {
          if (_context.Submission == null)
          {
              return NotFound();
          }
            return await _context.Submission.ToListAsync();
        }

        // GET: api/SubmissionsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Submission>> GetSubmission(int id)
        {
          if (_context.Submission == null)
          {
              return NotFound();
          }
            var submission = await _context.Submission.FindAsync(id);

            if (submission == null)
            {
                return NotFound();
            }

            return submission;
        }

        // PUT: api/SubmissionsAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubmission(int id, Submission submission)
        {
            if (id != submission.SubmissionId)
            {
                return BadRequest();
            }

            _context.Entry(submission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SubmissionsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //[Route("submitBird")]
        public async Task<ActionResult<Submission>> PostSubmission(Submission submission)
        {
          if (_context.Submission == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Submission'  is null.");
          }

            //-----------------------------------------------------------------------
            //Save image to wwwroot/ image
            //string wwwRootPath = _hostEnvironment.WebRootPath;
            //string fileName = Path.GetFileNameWithoutExtension(submission.BirdImage.FileName);
            //string extension = Path.GetExtension(submission.BirdImage.FileName);
            //submission.FileName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //string path = Path.Combine(wwwRootPath + "/Photos/", fileName);
            //using (var fileStream = new FileStream(path, FileMode.Create))
            //{
            //    await submission.BirdImage.CopyToAsync(fileStream);
            //}

            //-----------------------------------------------------------------------
            _context.Submission.Add(submission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubmission", new { id = submission.SubmissionId }, submission);
        }

        // DELETE: api/SubmissionsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubmission(int id)
        {
            if (_context.Submission == null)
            {
                return NotFound();
            }
            var submission = await _context.Submission.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }

            _context.Submission.Remove(submission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubmissionExists(int id)
        {
            return (_context.Submission?.Any(e => e.SubmissionId == id)).GetValueOrDefault();
        }
    }
}
