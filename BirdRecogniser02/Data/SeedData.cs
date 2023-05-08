using BirdRecogniser02.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BirdRecogniser02.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@birdrecogniser.com");
                await EnsureRole(serviceProvider, adminID, Authorization.Constants.SubmissionAdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@birdrecogniser.com");
                await EnsureRole(serviceProvider, managerID, Authorization.Constants.SubmissionManagersRole);

                SeedDB(context, adminID);
            }

        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }
        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                              string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (context.Submission.Any())
            {
                return;   // DB has been seeded
            }

            context.Submission.AddRange(
                new Submission
                {
                    BirdName = "King Fisher",
                    BirdInformation = " blue and orange bird ",
                    FileName = "kingfisher-2046453__340.jpg",
                    Status = SubmissionStatus.Approved,
                    OwnerID = adminID
                },
                new Submission
                {
                    BirdName = "New bird",
                    BirdInformation = " mix colors bird ",
                    FileName = "NewBird.jpeg",
                    Status = SubmissionStatus.Submitted,
                    OwnerID = adminID
                }
             );
            context.SaveChanges();
        }
    }
}
