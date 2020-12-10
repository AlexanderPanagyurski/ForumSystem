namespace ForumSystem.Services.Data
{
    using ForumSystem.Web.ViewModels.Users;

    public interface IUsersService
    {
        TopUsersViewModel GetTopUsers();

        UserOwnPostsViewModel GetUserPosts(string userId, int? take = null, int skip = 0);
    }
}
