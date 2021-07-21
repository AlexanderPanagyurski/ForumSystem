using ForumSystem.Data;
using ForumSystem.Data.Repositories;
using ForumSystem.Services.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForumSystem.Services.Tests
{
    public class FavouritePostsTests
    {
        [Fact]
        public async Task CheckAddingPostToFavourites()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var mockRepository = new EfDeletableEntityRepository<FavoritePost>(new ApplicationDbContext(options.Options));
            var service = new FavoritesService(mockRepository);

            await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", "0a9ec75c-0560-4e5b-94d4-c44429bb7379");
            var postFavouritesCount = service.GetCount("d3946347-0005-45e0-8a02-ad8179d2ece6");

            Assert.Equal(1, postFavouritesCount);
        }

        [Fact]
        public async Task RemovePostsFromFavourites()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var mockRepository = new EfDeletableEntityRepository<FavoritePost>(new ApplicationDbContext(options.Options));
            var service = new FavoritesService(mockRepository);

            await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", "0a9ec75c-0560-4e5b-94d4-c44429bb7379");
            await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", "0a9ec75c-0560-4e5b-94d4-c44429bb7379");
            var postFavouritesCount = service.GetCount("d3946347-0005-45e0-8a02-ad8179d2ece6");

            Assert.Equal(0, postFavouritesCount);
        }
    }
}
