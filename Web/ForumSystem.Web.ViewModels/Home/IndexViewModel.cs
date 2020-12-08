namespace ForumSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.Posts;

    public class IndexViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<IndexCategoryViewModel> Categories { get; set; }

        public IEnumerable<TrendingPostViewModel> TrendingPosts { get; set; }
    }
}
