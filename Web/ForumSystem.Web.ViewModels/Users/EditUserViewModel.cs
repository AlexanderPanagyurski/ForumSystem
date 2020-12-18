namespace ForumSystem.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class EditUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        public string Address { get; set; }

        public string WebsiteUrl { get; set; }

        public string GithubUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string InstagramUrl { get; set; }

        public string FacebookUrl { get; set; }
    }
}
