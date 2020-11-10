namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Home;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<IndexCategoryViewModel> GetAll(int? count = null)
        {
            var categories = this.categoriesRepository.All().Select(x => new IndexCategoryViewModel
            {
                Name = x.Name,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
            })
                .ToArray();

            return categories;
        }
    }
}
