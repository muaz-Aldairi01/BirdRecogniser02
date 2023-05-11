using BirdRecogniser02.Authorization;
using BirdRecogniser02.Controllers;
using BirdRecogniser02.Data;
using BirdRecogniser02.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace BirdRecogniser02UnitTest
{
    [TestClass]
    public class SubmissionsUnitTest
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfSubmissions(Mock<ApplicationDbContext> mockContext)
        {
            // Arrange
            var mock1Context = new Mock<ApplicationDbContext>();
           // mock1Context.Setup(sub => sub.ListAsync())
        //.ReturnsAsync(GetTestSubmissions());
            var mockEnv = new Mock<IWebHostEnvironment>();
            var mockAuth = new Mock<IAuthorizationService>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var controller = new SubmissionsController(mock1Context.Object, mockEnv.Object, mockAuth.Object, mockUserManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Role, Constants.SubmissionManagersRole),
                //new Claim(ClaimTypes.Role, Constants.SubmissionAdminstratorsRole)
            }))
                }
            };

            // Act
            var result = await controller.Index();

            // Assert
            //Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.IsNull(viewResult.ViewName); // Optional: Check the view name if not the default view
            Assert.IsNotNull(viewResult.Model); // Optional: Check if the model is not null

        }

        private List<Submission> GetTestSubmissions()
        {
            var submissions = new List<Submission>();
            submissions.Add(new Submission()
            {
                BirdName = "King Fisher",
                BirdInformation = " blue and orange bird ",
                FileName = "kingfisher-2046453__340.jpg",
                Status = SubmissionStatus.Approved,
                OwnerID = "manager@birdrecogniser.com"
            });
            submissions.Add(new Submission()
            {
                BirdName = "New bird",
                BirdInformation = " mix colors bird ",
                FileName = "NewBird.jpeg",
                Status = SubmissionStatus.Submitted,
                OwnerID = "manager@birdrecogniser.com"
            });
            return submissions;
        }
    }
}
