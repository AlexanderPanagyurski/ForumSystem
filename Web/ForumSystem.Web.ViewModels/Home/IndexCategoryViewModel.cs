namespace ForumSystem.Web.ViewModels.Home
{
    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class IndexCategoryViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string Url => $"/c/{this.Name.Replace(' ', '-')}";

        public string ImageUrl { get; set; }

        public int PostsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Category, IndexCategoryViewModel>()
                .ForMember(x => x.Description, options =>
                {
                    options.MapFrom(x => (x.Description.Length > 10) ? x.Description.Substring(0, 10) + "..." : x.Description);
                });
        }
    }
}
