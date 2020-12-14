namespace ForumSystem.Web.Controllers
{
    using System;

    using ForumSystem.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        private const int ItemsPerPage = 5;

        private readonly IUsersService usersService;
        private readonly IPostsService postsService;

        public UsersController(IUsersService usersService, IPostsService postsService)
        {
            this.usersService = usersService;
            this.postsService = postsService;
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
    }
}
