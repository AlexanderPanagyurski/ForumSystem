namespace ForumSystem.Web.ViewModels.Posts
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryDropDownViewModel : IMapFrom<Category>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
