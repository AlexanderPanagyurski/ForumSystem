﻿
namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.Categories;

    public class UserPostsViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<PostInCategoryViewModel> FavoritePosts { get; set; }
    }
}
