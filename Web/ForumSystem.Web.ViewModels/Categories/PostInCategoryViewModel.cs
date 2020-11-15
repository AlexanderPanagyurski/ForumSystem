namespace ForumSystem.Web.ViewModels.Categories
{
    using System;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class PostInCategoryViewModel : IMapFrom<Post>
    {
        public string Title { get; set; }

        public string UserUserName { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CommentsCount { get; set; }
    }
}
