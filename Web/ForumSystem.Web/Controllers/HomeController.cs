namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;
    using ForumSystem.Services.Data;
    using ForumSystem.Services.Messaging;
    using ForumSystem.Web.ViewModels;
    using ForumSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private const int ItemsPerPage = 8;

        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;
        private readonly IEmailSender emailSender;

        public HomeController(
            ICategoriesService categoriesService,
            IPostsService postsService,
            IEmailSender emailSender)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
            this.emailSender = emailSender;
        }

        public IActionResult Index(int page = 1)
        {
            var viewModel = new IndexViewModel
            {
                Categories =
                    this.categoriesService.GetAll<IndexCategoryViewModel>(ItemsPerPage, (page - 1) * ItemsPerPage),
                TrendingPosts = this.postsService.GetTrendingPosts(),
            };
            var count = this.categoriesService.GetCategoriesCount();
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public IActionResult Contacts()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contacts(ContactsViewModel viewModel)
        {
            await this.emailSender.SendEmailAsync(viewModel.Email, viewModel.Name, "alexander.panagyurski@gmail.com", viewModel.Subject, viewModel.Message);
            return this.Redirect("/Home/Index");
        }

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult StatusCodeError(int errorCode)
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
