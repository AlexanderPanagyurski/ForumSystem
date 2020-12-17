namespace ForumSystem.Web.ViewModels.Users
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class EditUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string WebsiteUrl { get; set; }

        public string GithubUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string InstagramUrl { get; set; }

        public string FacebookUrl { get; set; }
    }
}
