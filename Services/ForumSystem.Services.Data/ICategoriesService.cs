namespace ForumSystem.Services.Data
{
    using System.Collections.Generic;

    using ForumSystem.Web.ViewModels.Home;

    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetByName<T>(string name);

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
