namespace ForumSystem.Web.ViewModels.Users
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class UserProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string ProfileImage { get; set; }

        public string WebsiteUrl { get; set; }

        public string GithubUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string InstagramUrl { get; set; }

        public string FacebookUrl { get; set; }

        public IEnumerable<UserTopPostsViewModel> TopPosts { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserProfileViewModel>()
                .ForMember(x => x.ProfileImage, options =>
                   {
                       options.MapFrom(x => (x.UserImages.FirstOrDefault() != null) ? "/images/users/" + x.UserImages.FirstOrDefault().Id + "." + x.UserImages.FirstOrDefault().Extension : "/images/users/default-profile-icon.jpg");
                   })
                .ForMember(x => x.TopPosts, options =>
                   {
                       options.MapFrom(x => x.Posts
                       .OrderByDescending(x => x.Votes.Sum(v => (int)v.VoteType))
                       .ThenByDescending(x => x.Comments.Count())
                       .ThenByDescending(x => x.CreatedOn)
                       .Take(12)
                       .ToArray());
                   });
        }
    }
}
