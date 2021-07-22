using ForumSystem.Data;
using ForumSystem.Data.Models;
using ForumSystem.Data.Repositories;
using ForumSystem.Services.Data;
using ForumSystem.Web.ViewModels.Comments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForumSystem.Services.Tests
{
    public class CommentsServicesTests
    {
        [Fact]
        public async Task CreateMethodShouldAddCorrectNewCommentToDbAndToArticle()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var commentsService = new EfDeletableEntityRepository<Comment>(dbContext);

            var comment = new Comment
            {
                Content = "testContent",
                UserId = "AlexPanagyurski99",
            };

            var commentList = new List<Comment>();
            commentList.Add(comment);

            var postId = Guid.NewGuid().ToString();
            var post = new Post
            {
                Id = postId,
                Comments = commentList,
            };

            await dbContext.Posts.AddAsync(post);

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "AlexPanagyurski99",
            };

            await dbContext.Users.AddAsync(user);

            var commentToAdd = new Comment
            {
                PostId = postId,
                Content = "testContent",
            };

            await commentsService.AddAsync(commentToAdd);
            await dbContext.SaveChangesAsync();

            Assert.NotNull(dbContext.Comments.FirstOrDefaultAsync());
            Assert.Equal("testContent", dbContext.Comments.FirstAsync().Result.Content);
            Assert.Equal("AlexPanagyurski99", dbContext.Comments.FirstAsync().Result.UserId);
        }
    }
}
