namespace ForumSystem.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Categories;
    using ForumSystem.Web.ViewModels.Home;
    using ForumSystem.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;

    public class UsersService : IUsersService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg", "PNG" };
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Post> postsRepository,
            UserManager<ApplicationUser> _userManager)
        {
            this.usersRepository = usersRepository;
            this.postsRepository = postsRepository;
            this.userManager = _userManager;
        }

        public AllUsersViewModel GetAllUsers(int? take = null, int skip = 0)
        {
            AllUsersViewModel viewModel = null;

            if (take.HasValue)
            {
                viewModel = new AllUsersViewModel
                {
                    AllUsers = this.usersRepository
                        .All()
                        .OrderByDescending(x => x.Posts.Count())
                        .Select(x => new UserViewModel
                        {
                            Email = x.Email,
                            PostsCount = x.Posts.Count(),
                            ProfileImage = (x.UserImages.OrderByDescending(x => x.CreatedOn).FirstOrDefault() != null) ? "/images/users/" + x.UserImages.OrderByDescending(x => x.CreatedOn).FirstOrDefault().Id + "." + x.UserImages.OrderByDescending(x => x.CreatedOn).FirstOrDefault().Extension : "/images/users/default-profile-icon.jpg",
                            UserId = x.Id,
                            UserUserName = x.UserName,
                        })
                        .Skip(skip)
                        .Take(take.Value)
                        .ToArray(),
                };
            }
            else
            {
                viewModel = new AllUsersViewModel
                {
                    AllUsers = this.usersRepository
                        .All()
                        .Select(x => new UserViewModel
                        {
                            Email = x.Email,
                            PostsCount = x.Posts.Count(),
                            ProfileImage = (x.UserImages.OrderByDescending(x => x.CreatedOn).FirstOrDefault() != null) ? "/images/users/" + x.UserImages.OrderByDescending(x => x.CreatedOn).FirstOrDefault().Id + "." + x.UserImages.OrderByDescending(x => x.CreatedOn).FirstOrDefault().Extension : "/images/users/default-profile-icon.jpg",
                            UserId = x.Id,
                            UserUserName = x.UserName,
                        })
                        .ToArray(),
                };
            }

            return viewModel;
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
                    Email = x.Email,
                    PostsCount = x.Posts.Count(),
                    ProfileImage = (x.UserImages.OrderByDescending(x => x.CreatedOn).FirstOrDefault() != null) ? "/images/users/" + x.UserImages.OrderByDescending(x => x.CreatedOn).FirstOrDefault().Id + "." + x.UserImages.OrderByDescending(x => x.CreatedOn).FirstOrDefault().Extension : "/images/users/default-profile-icon.jpg",
                })
                .ToArray(),
            };

            return viewModel;
        }

        public UserOwnPostsViewModel GetUserPosts(string userId, int? take = null, int skip = 0)
        {
            UserOwnPostsViewModel viewModel = null;

            if (take.HasValue)
            {
                viewModel = new UserOwnPostsViewModel
                {
                    UserPosts = this.postsRepository
                        .All()
                        .Where(x => x.UserId == userId)
                        .OrderByDescending(x => x.CreatedOn)
                        .Select(x => new PostInCategoryViewModel
                        {
                            CategoryName = x.Category.Name,
                            CommentsCount = x.Comments.Count(),
                            Content = x.Content,
                            UserUserName = x.User.UserName,
                            CreatedOn = x.CreatedOn,
                            Id = x.Id,
                            Title = x.Title,
                        })
                        .Skip(skip)
                        .Take(take.Value)
                        .ToArray(),
                };
            }
            else
            {
                viewModel = new UserOwnPostsViewModel
                {
                    UserPosts = this.postsRepository
                        .All()
                        .Where(x => x.UserId == userId)
                        .OrderByDescending(x => x.CreatedOn)
                        .Select(x => new PostInCategoryViewModel
                        {
                            CategoryName = x.Category.Name,
                            CommentsCount = x.Comments.Count(),
                            Content = x.Content,
                            UserUserName = x.User.UserName,
                            CreatedOn = x.CreatedOn,
                            Id = x.Id,
                            Title = x.Title,
                        })
                        .ToArray(),
                };
            }

            return viewModel;
        }

        public UserProfileViewModel GetUserProfile(string userId)
        {
            var viewModel = this.usersRepository.All().Where(x => x.Id == userId).To<UserProfileViewModel>().FirstOrDefault();

            return viewModel;
        }

        public EditUserViewModel GetEditUserProfile(string userId)
        {
            var viewModel = this.usersRepository.All().Where(x => x.Id == userId).To<EditUserViewModel>().FirstOrDefault();

            return viewModel;
        }

        public int GetUsersCount()
        {
            return this.usersRepository.All().Count();
        }

        public async Task UpdateAsync(string userId, EditUserViewModel input, string imagePath)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            user.UserName = input.UserName;
            user.NormalizedUserName = input.UserName.Normalize().ToUpper();
            user.Email = input.Email;
            user.NormalizedEmail = input.Email.Normalize().ToUpper();
            user.PhoneNumber = input.PhoneNumber;
            user.Address = input.Address;
            user.ModifiedOn = DateTime.UtcNow;
            user.WebsiteUrl = input.WebsiteUrl;
            user.GithubUrl = input.GithubUrl;
            user.TwitterUrl = input.TwitterUrl;
            user.InstagramUrl = input.InstagramUrl;
            user.FacebookUrl = input.FacebookUrl;
            if (input.Password != null)
            {
                await this.userManager.RemovePasswordAsync(user);
                await this.userManager.AddPasswordAsync(user, input.Password);
            }

            Directory.CreateDirectory($"{imagePath}/posts/");
            if (input.UserUserImages != null)
            {
                foreach (var image in input.UserUserImages)
                {
                    var extension = Path.GetExtension(image.FileName).TrimStart('.');
                    if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                    {
                        throw new ArgumentException($"Invalid image extension {extension}");
                    }

                    var dbImage = new UserImage
                    {
                        UserId = userId,
                        Extension = extension,
                    };
                    user.UserImages.Add(dbImage);
                    var physicalPath = $"{imagePath}/users/{dbImage.Id}.{extension}";
                    using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                    await image.CopyToAsync(fileStream);
                }

                await this.usersRepository.SaveChangesAsync();
            }
        }

        public ContactsViewModel GetUserInfo(string userId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);

            ContactsViewModel viewModel = new ContactsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
            };

            return viewModel;
        }

        public async Task BanUserAsync(string id)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == id);
            this.usersRepository.Delete(user);
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
