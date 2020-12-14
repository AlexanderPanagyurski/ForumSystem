﻿namespace ForumSystem.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Categories;
    using ForumSystem.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository, IDeletableEntityRepository<Post> postsRepository)
        {
            this.usersRepository = usersRepository;
            this.postsRepository = postsRepository;
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
                            ProfileImage = (x.UserImages.FirstOrDefault() != null) ? "/images/users/" + x.UserImages.FirstOrDefault().Id + "." + x.UserImages.FirstOrDefault().Extension : "/images/users/default-profile-icon.jpg",
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
                            ProfileImage = (x.UserImages.FirstOrDefault() != null) ? "/images/users/" + x.UserImages.FirstOrDefault().Id + "." + x.UserImages.FirstOrDefault().Extension : "/images/users/default-profile-icon.jpg",
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
                    ProfileImage = (x.UserImages.FirstOrDefault() != null) ? "/images/users/" + x.UserImages.FirstOrDefault().Id + "." + x.UserImages.FirstOrDefault().Extension : "/images/users/default-profile-icon.jpg",
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

        public async Task UpdateAsync(string userId, EditUserViewModel input)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            user.UserName = input.UserName;
            user.Email = input.Email;
            user.PhoneNumber = input.PhoneNumber;
            user.Address = input.Address;
            user.ModifiedOn = DateTime.UtcNow;

            await this.usersRepository.SaveChangesAsync();
        }
    }
}
