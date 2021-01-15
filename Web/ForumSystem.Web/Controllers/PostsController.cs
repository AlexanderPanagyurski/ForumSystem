namespace ForumSystem.Web.Controllers
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

        [HttpGet]
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

        public async Task<IActionResult> ById(string id)
        {
            var postViewModel = this.postsService.GetById<PostViewModel>(id);

            if (postViewModel == null)
            {
                return this.Redirect("Home/StatusCodeError");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                postViewModel.IsOwner = false;
            }
            else
            {
                postViewModel.IsOwner = postViewModel.UserId == user.Id;
            }

            return this.View(postViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateInputModel input)
        {
            var id = string.Empty;

            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid || input.Content == null)
            {
                return this.RedirectToAction(nameof(this.Create));
            }

            try
            {
                id = await this.postsService.CreateAsync(input, user.Id, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.RedirectToAction(nameof(this.Create));
            }

            this.TempData["InfoMessage"] = "Forum post created!";
            return this.RedirectToAction(nameof(this.ById), new { id });
        }

        public async Task<IActionResult> GetFavoritesPosts(string orderBy, int page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.postsService.GetFavoritesPosts(user.Id, ItemsPerPage, (page - 1) * ItemsPerPage);
            var count = this.postsService.GetCountByUserFavoritePosts(user.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public async Task<IActionResult> GetMyPosts(string orderBy, int page = 1)
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

        public async Task<IActionResult> GetSearchedPosts(string title, int page = 1)
        {
            var viewModel = this.postsService.GetSearchedPosts(title, ItemsPerPage, (page - 1) * ItemsPerPage);
            var count = this.postsService.GetCountByPostsBySearch(title);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;
            viewModel.Title = title;

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var postViewModel = this.postsService.GetById<PostViewModel>(id);
            if (this.userManager.GetUserId(this.User) != postViewModel.UserId
                && !this.User.IsInRole(ForumSystem.Common.GlobalConstants.AdministratorRoleName))
            {
                this.TempData["InfoMessage"] = "You are not authorized to edit this post.";
                return this.Redirect("/Home/Index");
            }

            var inputModel = this.postsService.GetById<EditPostViewModel>(id);
            inputModel.Categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();
            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(string id, EditPostViewModel post)
        {
            var postViewModel = this.postsService.GetById<PostViewModel>(id);
            if (this.userManager.GetUserId(this.User) != postViewModel.UserId
                && !this.User.IsInRole(ForumSystem.Common.GlobalConstants.AdministratorRoleName))
            {
                this.TempData["InfoMessage"] = "You are not authorized to edit this post.";
                return this.Redirect("/Home/Index");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.postsService.UpdateAsync(id, post);
            this.TempData["InfoMessage"] = "Successfully edited post.";
            return this.RedirectToAction(nameof(this.ById), new { id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var post = this.postsService.GetById<PostViewModel>(id);
            if (this.userManager.GetUserId(this.User) != post.UserId
                && !this.User.IsInRole(ForumSystem.Common.GlobalConstants.AdministratorRoleName))
            {
                this.TempData["InfoMessage"] = "You are not authorized to delete this post.";
                return this.Redirect("/Home/Index");
            }

            await this.postsService.DeleteAsync(id);
            this.TempData["InfoMessage"] = "Successfully deleted post.";
            return this.Redirect("/Home/Index");
        }
    }
}
