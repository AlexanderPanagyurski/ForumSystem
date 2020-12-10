namespace ForumSystem.Services.Data
{
    using System.Linq;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public TopUsersViewModel GetTopUsers()
        {
            var viewModel = new TopUsersViewModel
            {
                TopUsers = this.usersRepository
                .All()
                .Where(x => x.Posts.Count > 0)
                .OrderByDescending(x => x.Posts.Count())
                .Take(12)
                .Select(x => new UserViewModel
                {
                    UserId = x.Id,
                    UserUserName = x.UserName,
                    Email=x.Email,
                    PostsCount = x.Posts.Count(),
                    ProfileImage = (x.UserImages.FirstOrDefault() != null) ? "/images/users/" + x.UserImages.FirstOrDefault().Id + "." + x.UserImages.FirstOrDefault().Extension : "/images/users/default-profile-icon.jpg",
                })
                .ToArray(),
            };

            return viewModel;
        }
    }
}
