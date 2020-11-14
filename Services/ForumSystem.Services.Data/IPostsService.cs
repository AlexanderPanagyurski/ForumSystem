namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostsService
    {
        Task<string> CreateAsync(string title, string content, string categoryId, string userId);

        T GetById<T>(string id);

        IEnumerable<T> GetByCategoryId<T>(string categoryId, int? take = null, int skip = 0);

        int GetCountByCategoryId(string categoryId);
    }
}
