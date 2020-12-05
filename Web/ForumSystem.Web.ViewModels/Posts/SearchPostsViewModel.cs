namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.Categories;

    public class SearchPostsViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public string Title { get; set; }

        public IEnumerable<PostInCategoryViewModel> SearchPosts { get; set; }
    }
}
