namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface IFavoritesService
    {
        Task FavorAsync(string postId, string userId);

        int GetCount(string postId);
    }
}
