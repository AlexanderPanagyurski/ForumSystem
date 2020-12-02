﻿namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : Controller
    {
        private const int ItemsPerPage = 5;

        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.environment = environment;
        }

        [Authorize]
        public IActionResult Create()
        {
            var categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();
            var viewModel = new PostCreateInputModel
            {
                Categories = categories,
            };
            return this.View(viewModel);
        }

        public IActionResult ById(string id)
        {
            var postViewModel = this.postsService.GetById<PostViewModel>(id);
            if (postViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(postViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                var postId = await this.postsService.CreateAsync(input, user.Id, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(input);
            }

            this.TempData["InfoMessage"] = "Forum post created!";
            return this.Redirect("/");
        }

        public async Task<IActionResult> GetFavoritesPosts(int page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.postsService.GetFavoritesPosts(user.Id, ItemsPerPage, (page - 1) * ItemsPerPage);
            var count = this.postsService.GetCountByUserFavoritePosts(user.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public async Task<IActionResult> GetMyPosts(int page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.postsService.GetUserPosts(user.Id, ItemsPerPage, (page - 1) * ItemsPerPage);
            var count = this.postsService.GetCountByUserPosts(user.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public async Task<IActionResult> GetPopularPosts(int page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.postsService.GetPopularPosts(ItemsPerPage, (page - 1) * ItemsPerPage);
            var count = 10;
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }
    }
}
