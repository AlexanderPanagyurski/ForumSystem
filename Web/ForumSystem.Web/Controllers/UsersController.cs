﻿namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ForumSystem.Common;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Services.Messaging;
    using ForumSystem.Web.ViewModels.Home;
    using ForumSystem.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        private const int ItemsPerPage = 5;

        private readonly IUsersService usersService;
        private readonly IPostsService postsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment environment;

        public UsersController(
            IUsersService usersService,
            IPostsService postsService,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IWebHostEnvironment environment)
        {
            this.usersService = usersService;
            this.postsService = postsService;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.environment = environment;
        }

        public IActionResult GetTopUsers()
        {
            var viewModel = this.usersService.GetTopUsers();

            return this.View(viewModel);
        }

        public IActionResult GetUserPosts(string userId, int page = 1)
        {
            var viewModel = this.usersService.GetUserPosts(userId, ItemsPerPage, (page - 1) * ItemsPerPage);
            viewModel.Id = userId;
            var count = this.postsService.GetCountByUserPosts(userId);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;
            return this.View(viewModel);
        }

        public IActionResult GetAllUsers(int page = 1)
        {
            var viewModel = this.usersService.GetAllUsers(6, (page - 1) * 6);
            var count = this.usersService.GetUsersCount();
            viewModel.PagesCount = (int)Math.Ceiling((double)count / 6);
            viewModel.CurrentPage = page;
            return this.View(viewModel);
        }

        public IActionResult UserProfile(string userId)
        {
            var viewModel = this.usersService.GetUserProfile(userId);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> EditUser(string userId)
        {
            if (this.userManager.GetUserId(this.User) != userId
                && !this.User.IsInRole(ForumSystem.Common.GlobalConstants.AdministratorRoleName))
            {
                return this.Redirect("/Home/Index");
            }

            var userViewModel = this.usersService.GetUserProfile(userId);
            var user = await this.userManager.GetUserAsync(this.User);

            EditUserViewModel inputModel = this.usersService.GetEditUserProfile(userId);
            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditUser(string userId, EditUserViewModel user)
        {
            if (this.userManager.GetUserId(this.User) != userId
                && !this.User.IsInRole(ForumSystem.Common.GlobalConstants.AdministratorRoleName))
            {
                return this.Redirect("/Home/Index");
            }

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.EditUser), new { userId });
            }

            try
            {
                await this.usersService.UpdateAsync(userId, user, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.RedirectToAction(nameof(this.EditUser));
            }

            this.TempData["InfoMessage"] = "Successfully updated your profile!";
            return this.RedirectToAction(nameof(this.UserProfile), new { userId });
        }

        public IActionResult SendEmail(string id)
        {
            var viewModel = this.usersService.GetUserInfo(id);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string id, ContactsViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.SendEmail), new { id });
            }

            var user = this.usersService.GetUserInfo(id);
            await this.emailSender.SendEmailAsync("alexander.panagyurski@gmail.com", viewModel.Name, user.Email, viewModel.Subject, viewModel.Message);

            return this.Redirect("/Home/Index");
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> BanUser(string id)
        {
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Redirect("Home/Index");
            }

            await this.usersService.BanUserAsync(id);

            this.TempData["InfoMessage"] = "Successfully banned user";
            return this.Redirect("/Home/Index");
        }
    }
}
