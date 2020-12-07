namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.Home;

    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0, int? count = null);

        T GetByName<T>(string name);

        int GetCategoriesCount();

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
