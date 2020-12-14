namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        private const int ItemsPerPage = 5;

        private readonly IUsersService usersService;
        private readonly IPostsService postsService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            IUsersService usersService,
            IPostsService postsService,
            UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.postsService = postsService;
            this.userManager = userManager;
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
            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> EditUser(string userId)
        {
            var userViewModel = this.usersService.GetUserProfile(userId);
            var user = await this.userManager.GetUserAsync(this.User);
            //if (user.Id != userViewModel.Id)
            //{
            //    return this.Redirect("/Home/Index");
            //}

            EditUserViewModel inputModel = this.usersService.GetEditUserProfile(userId);
            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditUser(string userId, EditUserViewModel user)
        {
            //if (!this.ModelState.IsValid)
            //{
            //    return this.View();
            //}

            await this.usersService.UpdateAsync(userId, user);
            return this.RedirectToAction(nameof(this.UserProfile), new { userId });
        }
    }
}
