using BirdRecogniser02.Authorization;
using BirdRecogniser02.Controllers;
using BirdRecogniser02.Data;
using BirdRecogniser02.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
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

        
    }
}