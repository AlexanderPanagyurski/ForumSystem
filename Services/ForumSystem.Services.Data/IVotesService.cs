namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task VoteAsync(string postId, string userId, bool isUpVote);

        int GetVotes(string postId);
    }
}
