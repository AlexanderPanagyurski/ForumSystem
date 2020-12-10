namespace ForumSystem.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.Categories;

    public class UserOwnPostsViewModel
    {
        public string Id { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<PostInCategoryViewModel> UserPosts { get; set; }
    }
}
