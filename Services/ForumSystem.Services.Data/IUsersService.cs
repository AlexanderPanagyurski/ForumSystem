namespace ForumSystem.Services.Data
{
    using ForumSystem.Web.ViewModels.Users;

    public interface IUsersService
    {
        TopUsersViewModel GetTopUsers();

        AllUsersViewModel GetAllUsers(int? take = null, int skip = 0);

        int GetUsersCount();

        UserOwnPostsViewModel GetUserPosts(string userId, int? take = null, int skip = 0);

        UserProfileViewModel GetUserProfile(string userId);
    }
}
