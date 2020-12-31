namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;
    using ForumSystem.Web.ViewModels.Home;
    using ForumSystem.Web.ViewModels.Users;

    public interface IUsersService
    {
        TopUsersViewModel GetTopUsers();

        AllUsersViewModel GetAllUsers(int? take = null, int skip = 0);

        int GetUsersCount();

        UserOwnPostsViewModel GetUserPosts(string userId, int? take = null, int skip = 0);

        UserProfileViewModel GetUserProfile(string userId);

        Task UpdateAsync(string userId, EditUserViewModel input, string imagePath);

        EditUserViewModel GetEditUserProfile(string userId);

        ContactsViewModel GetUserInfo(string userId);

        Task BanUserAsync(string id);
    }
}
