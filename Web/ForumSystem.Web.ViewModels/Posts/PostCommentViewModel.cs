namespace ForumSystem.Web.ViewModels.Posts
{
    using System;
    using System.Linq;
    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Ganss.XSS;

    public class PostCommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string ParentId { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserUserName { get; set; }

        public string UserId { get; set; }

        public string UserProfileImage { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, PostCommentViewModel>()
                .ForMember(x => x.UserProfileImage, options =>
            {
                options.MapFrom(x => (x.User.UserImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ?
                   "/images/users/" + x.User.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." + x.User.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : "/images/users/default-profile-icon.jpg");
            });
        }
    }
}
