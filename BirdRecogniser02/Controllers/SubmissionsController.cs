using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BirdRecogniser02.Data;
using BirdRecogniser02.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BirdRecogniser02.Authorization;

namespace BirdRecogniser02.Controllers
{


    public class SubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;
        public SubmissionsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        // GET: Submissions
        public async Task<IActionResult> Index()
        {

            //-----------------------------------------------------
            var submission = from s in _context.Submission
                              select s;

            var isAuthorized = User.IsInRole(Constants.SubmissionManagersRole) ||
                               User.IsInRole(Constants.SubmissionAdministratorsRole);

            var currentUserId = _userManager.GetUserId(User);

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                submission = submission.Where(s => s.Status == SubmissionStatus.Approved ||
                                             s.OwnerID == currentUserId);
            }

            //-----------------------------------------------------------------------
            //return _context.Submission != null ? 
            //              View(await _context.Submission.ToListAsync()) :
            //              Problem("Entity set 'ApplicationDbContext.Submission'  is null.");

            return submission != null ?
                   View(await submission.ToListAsync()) :
                   Problem("Entity set 'ApplicationDbContext.Submission'  is null.");
        }

        // GET: Submissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Submission == null)
            {
                return NotFound();
            }

            var submission = await _context.Submission
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (submission == null)
            {
                return NotFound();
            }

            //-----------------------------------------------------------
            var isAuthorized = User.IsInRole(Constants.SubmissionManagersRole) ||
                           User.IsInRole(Constants.SubmissionAdministratorsRole);

            var currentUserId = _userManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != submission.OwnerID
                && submission.Status != SubmissionStatus.Approved)
            {
                return Forbid();
            }
            //-----------------------------------------------------------

            return View(submission);
        }
        //------------------------------------------------------------------------------

        //// POST: Submissions/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int? id, SubmissionStatus status)
        {
            var submission = await _context.Submission.FirstOrDefaultAsync(m => m.SubmissionId == id);

            if (submission == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var submissionOperation = (status == SubmissionStatus.Approved)
                                                   ? SubmissionOperations.Approve
                                                   : SubmissionOperations.Reject;

                var isAuthorized = await _authorizationService.AuthorizeAsync(User, submission,
                                            submissionOperation);
                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                submission.Status = status;

                _context.Submission.Update(submission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(submission);
        }


        //------------------------------------------------------------------------------
        // GET: Submissions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Submissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubmissionId,OwnerID,BirdName,BirdInformation,FileName,BirdImage,Status")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(submission.BirdImage.FileName);
                string extension = Path.GetExtension(submission.BirdImage.FileName);
                submission.FileName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Photos/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await submission.BirdImage.CopyToAsync(fileStream);
                }

                //--------------------------------------------------------
                submission.OwnerID = _userManager.GetUserId(User);

                var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                            User, submission,
                                                            SubmissionOperations.Create);
                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                //------------------------------------------------------------------------
                _context.Add(submission);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Thanks");
            }
            return View(submission);
        }
        //create thanks return view
        public IActionResult Thanks()
        {
            return View();
        }

        // GET: Submissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Submission == null)
            {
                return NotFound();
            }

            var submission = await _context.Submission.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }
            //----------------------------------------------------------
            // you may need to add two more perameters to get method (bind and submission)
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                  User, submission,
                                                  SubmissionOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            //-------------------------------------------------------------------
            return View(submission);
        }

        // POST: Submissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubmissionId,OwnerID,BirdName,BirdInformation,FileName,BirdImage,Status")] Submission submission)
        {
            if (id != submission.SubmissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(submission.BirdImage.FileName);
                    string extension = Path.GetExtension(submission.BirdImage.FileName);
                    submission.FileName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Photos/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await submission.BirdImage.CopyToAsync(fileStream);
                    }

                    //------------------------------------------------------------------

                    var existingSubmission = await _context.Submission.AsNoTracking().FirstOrDefaultAsync(m => m.SubmissionId == id);

                    if (existingSubmission == null)
                    {
                        return NotFound();
                    }

                    var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                 User, existingSubmission,
                                                 SubmissionOperations.Update);
                    if (!isAuthorized.Succeeded)
                    {
                        return Forbid();
                    }
                     submission.OwnerID = existingSubmission.OwnerID;
                    _context.Attach(submission).State = EntityState.Modified;

                    if (submission.Status == SubmissionStatus.Approved)
                    {
                        // If the contact is updated after approval, 
                        // and the user cannot approve,
                        // set the status back to submitted so the update can be
                        // checked and approved.
                        var canApprove = await _authorizationService.AuthorizeAsync(User,
                                                submission,
                                                SubmissionOperations.Approve);

                        if (!canApprove.Succeeded)
                        {
                            submission.Status = SubmissionStatus.Submitted;
                        }
                    }

                    //--------------------------------------------------------------------

                    _context.Update(submission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubmissionExists(submission.SubmissionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(submission);
        }

        // GET: Submissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Submission == null)
            {
                return NotFound();
            }

            var submission = await _context.Submission
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (submission == null)
            {
                return NotFound();
            }
            //--------------------------------------------------------------------
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                 User, submission,
                                                 SubmissionOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            //----------------------------------------------------------------------
            return View(submission);
        }

        // POST: Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Submission == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Submission'  is null.");
            }
            var submission = await _context.Submission.FindAsync(id);
            if (submission != null)
            {
                _context.Submission.Remove(submission);
            }
            //--------------------------------------------------------------------------
            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                 User, submission,
                                                 SubmissionOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            //--------------------------------------------------------------------------
            //delete image from wwwroot/image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "photos", submission.FileName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubmissionExists(int id)
        {
          return (_context.Submission?.Any(e => e.SubmissionId == id)).GetValueOrDefault();
        }
    }
}
