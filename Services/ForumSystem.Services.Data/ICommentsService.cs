namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task Create(string postId, string userId, string content, string parentId = null);

        bool IsInPostId(string commentId, string postId);
    }
}
