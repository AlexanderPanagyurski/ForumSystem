namespace ForumSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Categories;
    using ForumSystem.Web.ViewModels.Posts;

    public class PostsService : IPostsService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };

        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<UserImage> imagesRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<UserImage> imagesRepository)
        {
            this.postsRepository = postsRepository;
            this.imagesRepository = imagesRepository;
        }

        public async Task<string> CreateAsync(PostCreateInputModel input, string userId, string imagePath)
        {
            var post = new Post
            {
                CategoryId = input.CategoryId,
                Content = input.Content,
                Title = input.Title,
                UserId = userId,
            };
            Directory.CreateDirectory($"{imagePath}/posts/");
            if (input.Images != null)
            {
                foreach (var image in input.Images)
                {
                    var extension = Path.GetExtension(image.FileName).TrimStart('.');
                    if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                    {
                        throw new ArgumentException($"Invalid image extension {extension}");
                    }

                    var dbImage = new Image
                    {
                        UserId = userId,
                        Post = post,
                        Extension = extension,
                    };
                    post.Images.Add(dbImage);
                    var physicalPath = $"{imagePath}/posts/{dbImage.Id}.{extension}";
                    using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                    await image.CopyToAsync(fileStream);
                }
            }

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
            return post.Id;
        }

        public IEnumerable<T> GetByCategoryId<T>(string categoryId, int? take = null, int skip = 0, string orderBy = "default")
        {
            IQueryable<Post> query = null;

            if (orderBy == "default")
            {
                query = this.postsRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.CategoryId == categoryId).Skip(skip);
            }
            else if (orderBy == "tileAscending")
            {
                query = this.postsRepository.All()
                .OrderBy(x => x.Title)
                .Where(x => x.CategoryId == categoryId).Skip(skip);
            }
            else if (orderBy == "tileDescending")
            {
                query = this.postsRepository.All()
                .OrderByDescending(x => x.Title)
                .Where(x => x.CategoryId == categoryId).Skip(skip);
            }
            else if (orderBy == "datetimeAscending")
            {
                query = this.postsRepository.All()
                .OrderBy(x => x.CreatedOn)
                .Where(x => x.CategoryId == categoryId).Skip(skip);
            }
            else if (orderBy == "datetimeDescending")
            {
                query = this.postsRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.CategoryId == categoryId).Skip(skip);
            }
            else if (orderBy == "commentsAscending")
            {
                query = this.postsRepository.All()
                .OrderBy(x => x.Comments.Count())
                .Where(x => x.CategoryId == categoryId).Skip(skip);
            }
            else if (orderBy == "commentsDescending")
            {
                query = this.postsRepository.All()
                .OrderByDescending(x => x.Comments.Count())
                .Where(x => x.CategoryId == categoryId).Skip(skip);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            var post = this.postsRepository.All().Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return post;
        }

        public int GetCountByCategoryId(string categoryId)
        {
            return this.postsRepository.All().Count(x => x.CategoryId == categoryId);
        }

        public int GetCountByPopularPosts()
        {
            throw new NotImplementedException();
        }

        public int GetCountByPostsBySearch(string title)
        {
            return this.postsRepository.All().Count(x => x.Title.ToUpper().Contains(title.ToUpper()));
        }

        public int GetCountByUserFavoritePosts(string userId)
        {
            return this.postsRepository.All().Where(x => x.FavoritePosts.Any(x => x.UserId == userId)).Count();
        }

        public int GetCountByUserPosts(string userId)
        {
            return this.postsRepository.All().Where(x => x.UserId == userId).Count();
        }

        public IEnumerable<T> GetFavoritesPosts<T>(string userId)
        {
            var favoritePosts = this.postsRepository.All()
                .Where(x => x.FavoritePosts.Any(x => x.UserId == userId));

            return favoritePosts.To<T>().ToArray();
        }

        public UserFavoritesPostsViewModel GetFavoritesPosts(string userId, int? take = null, int skip = 0)
        {
            UserFavoritesPostsViewModel userFavoritesPosts = null;

            if (take.HasValue)
            {
                userFavoritesPosts = new UserFavoritesPostsViewModel
                {
                    PagesCount = this.postsRepository.All().Where(x => x.FavoritePosts.Any(x => x.UserId == userId)).Count(),
                    FavoritePosts = this.postsRepository.All()
                  .Where(x => x.FavoritePosts.Any(x => x.UserId == userId))
                  .OrderByDescending(x => x.CreatedOn)
                  .Select(x => new PostInCategoryViewModel
                  {
                      Id = x.Id,
                      Content = x.Content,
                      CategoryName = x.Category.Name,
                      CreatedOn = x.CreatedOn,
                      UserUserName = x.User.UserName,
                      Title = x.Title,
                      CommentsCount = x.Comments.Count(),
                  })
                  .Skip(skip)
                  .Take(take.Value),
                };
            }
            else
            {
                userFavoritesPosts = new UserFavoritesPostsViewModel
                {
                    FavoritePosts = this.postsRepository.All()
                 .Where(x => x.FavoritePosts.Any(x => x.UserId == userId))
                 .OrderByDescending(x => x.CreatedOn)
                 .Select(x => new PostInCategoryViewModel
                 {
                     Id = x.Id,
                     Content = x.Content,
                     CategoryName = x.Category.Name,
                     CreatedOn = x.CreatedOn,
                     UserUserName = x.User.UserName,
                     Title = x.Title,
                     CommentsCount = x.Comments.Count(),
                 }),
                };
            }

            return userFavoritesPosts;
        }

        public PopularPostsViewModel GetPopularPosts(int? take = null, int skip = 0)
        {
            PopularPostsViewModel viewModel = null;

            if (take.HasValue)
            {
                viewModel = new PopularPostsViewModel
                {
                    PopularPosts = this.postsRepository.All()
                     .OrderByDescending(x => x.Votes.Sum(v => (int)v.VoteType))
                     .ThenByDescending(x => x.Comments.Count())
                     .ThenBy(x => x.CreatedOn)
                     .Select(x => new PostInCategoryViewModel
                     {
                         Id = x.Id,
                         CategoryName = x.Category.Name,
                         CommentsCount = x.Comments.Count(),
                         Content = x.Content,
                         Title = x.Title,
                         UserUserName = x.User.UserName,
                         CreatedOn = x.CreatedOn,
                     })
                     .Skip(skip)
                     .Take(take.Value),
                };
            }
            else
            {
                viewModel = new PopularPostsViewModel
                {
                    PopularPosts = this.postsRepository.All()
                     .OrderByDescending(x => x.Votes.Sum(v => (int)v.VoteType))
                     .ThenByDescending(x => x.Comments.Count())
                     .ThenBy(x => x.CreatedOn)
                     .Select(x => new PostInCategoryViewModel
                     {
                         Id = x.Id,
                         CategoryName = x.Category.Name,
                         CommentsCount = x.Comments.Count(),
                         Content = x.Content,
                         Title = x.Title,
                         UserUserName = x.User.UserName,
                         CreatedOn = x.CreatedOn,
                     }),
                };
            }

            return viewModel;
        }

        public SearchPostsViewModel GetSearchedPosts(string title, int? take = null, int skip = 0)
        {
            SearchPostsViewModel viewModel = null;

            if (take.HasValue)
            {
                viewModel = new SearchPostsViewModel
                {
                    SearchPosts = this.postsRepository.All()
                      .Where(x => x.Title.ToUpper().Contains(title.ToUpper()))
                      .OrderByDescending(x => x.CreatedOn)
                      .Select(x => new PostInCategoryViewModel
                      {
                          Id = x.Id,
                          CategoryName = x.Category.Name,
                          CommentsCount = x.Comments.Count(),
                          Content = x.Content,
                          Title = x.Title,
                          UserUserName = x.User.UserName,
                          CreatedOn = x.CreatedOn,
                      })
                      .Skip(skip)
                     .Take(take.Value)
                     .ToArray(),
                };
            }
            else
            {
                viewModel = new SearchPostsViewModel
                {
                    SearchPosts = this.postsRepository.All()
                      .Where(x => x.Title.ToUpper().Contains(title.ToUpper()))
                      .OrderByDescending(x => x.CreatedOn)
                      .Select(x => new PostInCategoryViewModel
                      {
                          Id = x.Id,
                          CategoryName = x.Category.Name,
                          CommentsCount = x.Comments.Count(),
                          Content = x.Content,
                          Title = x.Title,
                          UserUserName = x.User.UserName,
                          CreatedOn = x.CreatedOn,
                      })
                      .ToArray(),
                };
            }

            return viewModel;
        }

        public IEnumerable<TrendingPostViewModel> GetTrendingPosts()
        {
            var trendingPosts =
                this.postsRepository
                .All()
                .Where(x => x.CreatedOn.Day == DateTime.Today.Day)
                .OrderByDescending(x => x.Votes.Sum(v => (int)v.VoteType))
                .ThenByDescending(x => x.Comments.Count())
                .ThenByDescending(x => x.FavoritePosts.Count())
                .ThenByDescending(x => x.CreatedOn)
                .Take(4)
                .To<TrendingPostViewModel>();

            var count = trendingPosts.Count();

            return trendingPosts;
        }

        public UserPostsViewModel GetUserPosts(string userId, int? take = null, int skip = 0)
        {
            UserPostsViewModel userPosts = null;

            if (take.HasValue)
            {
                userPosts = new UserPostsViewModel
                {
                    PagesCount = this.postsRepository.All().Where(x => x.FavoritePosts.Any(x => x.UserId == userId)).Count(),
                    FavoritePosts = this.postsRepository.All()
                  .Where(x => x.UserId == userId)
                  .OrderByDescending(x => x.CreatedOn)
                  .Select(x => new PostInCategoryViewModel
                  {
                      Id = x.Id,
                      Content = x.Content,
                      CategoryName = x.Category.Name,
                      CreatedOn = x.CreatedOn,
                      UserUserName = x.User.UserName,
                      Title = x.Title,
                      CommentsCount = x.Comments.Count(),
                  })
                  .Skip(skip)
                  .Take(10),
                };
            }
            else
            {
                userPosts = new UserPostsViewModel
                {
                    PagesCount = this.postsRepository.All().Where(x => x.FavoritePosts.Any(x => x.UserId == userId)).Count(),
                    FavoritePosts = this.postsRepository.All()
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => new PostInCategoryViewModel
                    {
                        Id = x.Id,
                        Content = x.Content,
                        CategoryName = x.Category.Name,
                        CreatedOn = x.CreatedOn,
                        UserUserName = x.User.UserName,
                        Title = x.Title,
                        CommentsCount = x.Comments.Count(),
                    }),
                };
            }

            return userPosts;
        }
    }
}
