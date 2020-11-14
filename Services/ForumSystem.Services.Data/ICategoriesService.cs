namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.Home;

    public interface ICategoriesService
    {
        IEnumerable<IndexCategoryViewModel> GetAll(int? count = null);

        T GetByName<T>(string name);
    }
}
