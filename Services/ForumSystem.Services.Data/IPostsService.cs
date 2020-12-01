namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ForumSystem.Web.ViewModels.Posts;

    public interface IPostsService
    {
        Task<string> CreateAsync(PostCreateInputModel input, string userId, string imagePath);

        T GetById<T>(string id);

        IEnumerable<T> GetByCategoryId<T>(string categoryId, int? take = null, int skip = 0, string orderBy = "default");

        int GetCountByCategoryId(string categoryId);

        UserFavoritesPostsViewModel GetFavoritesPosts(string userId, int? take = null, int skip = 0);

        int GetCountByUserFavoritePosts(string userId);

        UserPostsViewModel GetUserPosts(string userId, int? take = null, int skip = 0);

        int GetCountByUserPosts(string userId);
    }
}
