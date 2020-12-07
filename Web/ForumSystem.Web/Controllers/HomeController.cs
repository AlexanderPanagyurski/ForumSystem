﻿namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Diagnostics;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels;
    using ForumSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private const int ItemsPerPage = 12;

        private readonly ICategoriesService categoriesService;

        public HomeController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Index(int page = 1)
        {
            var viewModel = new IndexViewModel
            {
                Categories =
                    this.categoriesService.GetAll<IndexCategoryViewModel>(ItemsPerPage, (page - 1) * ItemsPerPage),
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

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult Privacy()
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
