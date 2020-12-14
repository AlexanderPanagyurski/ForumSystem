namespace ForumSystem.Web.ViewModels.Users
{
    using System.Linq;

    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class UserProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string ProfileImage { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserProfileViewModel>()
                .ForMember(x => x.ProfileImage, options =>
                   {
                       options.MapFrom(x => (x.UserImages.FirstOrDefault() != null) ? "/images/users/" + x.UserImages.FirstOrDefault().Id + "." + x.UserImages.FirstOrDefault().Extension : "/images/users/default-profile-icon.jpg");
                   });
        }
    }
}
