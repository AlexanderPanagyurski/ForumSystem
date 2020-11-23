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
    using ForumSystem.Web.ViewModels.Posts;

    public class PostsService : IPostsService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        private readonly IDeletableEntityRepository<Post> postsRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository
            )
        {
            this.postsRepository = postsRepository;
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

        public IEnumerable<T> GetByCategoryId<T>(string categoryId, int? take = null, int skip = 0)
        {
            var query = this.postsRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.CategoryId == categoryId).Skip(skip);
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
    }
}
