namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class RandomPostsViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Thumbnail { get; set; }

        public string Title { get; set; }

        public string UserUserName { get; set; }

        public string Content { get; set; }

        public string ShortContent
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.Content, @"<[^>]+>", string.Empty));
                return content.Length > 50
                        ? content.Substring(0, 70) + "..."
                        : content;
            }
        }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, RandomPostsViewModel>()
                .ForMember(x => x.Thumbnail, options =>
                {
                    options.MapFrom(x => (x.Images.FirstOrDefault() != null) ?
                    "/images/posts/" + x.Images.FirstOrDefault().Id + "." +
                    x.Images.FirstOrDefault().Extension :
                    "/images/posts/no-image.jpg");
                });
        }
    }
}
