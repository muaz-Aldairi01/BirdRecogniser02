using BirdRecogniser02.Controllers;
using BirdRecogniser02.Data;
using BirdRecogniser02.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace BirdRecognise02UnitTest
{
    public class UnitTest1
    {
        //[Fact]
        //public async Task Index_ReturnsViewResult_WithSubmissionList()
        //{
        //    // Arrange
        //    var submissionList = new List<Submission> { new Submission(), new Submission() };

        //    var contextMock = new Mock<ApplicationDbContext>();
        //    contextMock.Setup(c => c.Submission).ReturnsDbSet(submissionList);

        //    var userManagerMock = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict);
        //    userManagerMock.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("1");

        //    var controller = new SubmissionsController(contextMock.Object, userManagerMock.Object);
        //    controller.ControllerContext = new ControllerContext();
        //    controller.ControllerContext.HttpContext = new DefaultHttpContext();
        //    controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        //    {
        //    new Claim(ClaimTypes.Role, Constants.SubmissionManagersRole),
        //}, "mock"));

        //    // Act
        //    var result = await controller.Index();

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsAssignableFrom<IEnumerable<Submission>>(viewResult.Model);
        //    Assert.Equal(submissionList.Count, model.Count());
        //}


        //================================================================================


        //private readonly ApplicationDbContext _context;
        //private readonly IWebHostEnvironment _hostEnvironment;
        //private readonly IAuthorizationService _authorizationService;
        //private readonly Mock<UserManager<IdentityUser>> _userManager;

        
        //public UnitTest1()
        //{
        //    // Initialize any dependencies required for the controller
        //    _context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase("TestDatabase")
        //        .Options);
        //    _hostEnvironment = Mock.Of<IWebHostEnvironment>();
        //    _authorizationService = Mock.Of<IAuthorizationService>();
        //    _userManager = new Mock<UserManager<IdentityUser>>(MockBehavior.Strict);
        //}

        //[Fact]
        //public async Task Index_ReturnsViewResultWithSubmissions()
        //{
        //    // Arrange
        //    var controller = new SubmissionsController(_context, _hostEnvironment, _authorizationService, _userManager);

        //    // Act
        //    var result = await controller.Index();

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsAssignableFrom<IEnumerable<Submission>>(viewResult.ViewData.Model);
        //    Assert.NotNull(model);
        //}

        //[Fact]
        //public async Task Details_WithValidId_ReturnsViewResultWithSubmission()
        //{
        //    // Arrange
        //    var submissionId = 1;
        //    var submission = new Submission { SubmissionId = submissionId };
        //    _context.Submission.Add(submission);
        //    _context.SaveChanges();
        //    var controller = new SubmissionsController(_context, _hostEnvironment, _authorizationService, _userManager);

        //    // Act
        //    var result = await controller.Details(submissionId);

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsType<Submission>(viewResult.ViewData.Model);
        //    Assert.Equal(submissionId, model.SubmissionId);
        //}

        //[Fact]
        //public async Task Details_WithNullId_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    int? submissionId = null;
        //    var controller = new SubmissionsController(_context, _hostEnvironment, _authorizationService, _userManager);

        //    // Act
        //    var result = await controller.Details(submissionId);

        //    // Assert
        //    Assert.IsType<NotFoundResult>(result);
        //}

        //[Fact]
        //public async Task Details_WithUnauthorizedUser_ReturnsForbidResult()
        //{
        //    // Arrange
        //    var submissionId = 1;
        //    var submission = new Submission { SubmissionId = submissionId };
        //    _context.Submission.Add(submission);
        //    _context.SaveChanges();
        //    var controller = new SubmissionsController(_context, _hostEnvironment, _authorizationService, _userManager);
        //    controller.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = new DefaultHttpContext
        //        {
        //            User = new ClaimsPrincipal(new ClaimsIdentity())
        //        }
        //    };

        //    // Act
        //    var result = await controller.Details(submissionId);

        //    // Assert
        //    Assert.IsType<ForbidResult>(result);
        //}

    }
}