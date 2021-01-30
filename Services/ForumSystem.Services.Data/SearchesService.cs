namespace ForumSystem.Services.Data
{
    using System.Linq;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;

    public class SearchesService : ISearchesService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public SearchesService(IDeletableEntityRepository<Post> postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public string[] Searches()
        {
            var titles = this.postsRepository
                .All()
                .Select(x => x.Title)
                .ToArray();

            return titles;
        }
    }
}
