namespace ForumSystem.Web.ViewModels.Users
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class UserTopPostsViewModel : IMapFrom<Post>
    {
        public string Id { get; set; }

        public string Title { get; set; }

    }
}
