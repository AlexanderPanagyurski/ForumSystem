using ForumSystem.Data;
using ForumSystem.Data.Repositories;
using ForumSystem.Services.Data;
using Microsoft.EntityFrameworkCore;
using System;
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

        [Fact]
        public async Task AddToFavouritesFromTwoDifferentUsers()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var mockRepository = new EfDeletableEntityRepository<FavoritePost>(new ApplicationDbContext(options.Options));
            var service = new FavoritesService(mockRepository);

            await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", "0a9ec75c-0560-4e5b-94d4-c44429bb7379");
            await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", "65c47bf7-18e6-423b-adee-6c9dbe71a382");
            var postFavouritesCount = service.GetCount("d3946347-0005-45e0-8a02-ad8179d2ece6");

            Assert.Equal(2, postFavouritesCount);
        }
        [Fact]
        public async Task AddToFavouritesFromTwoDifferentUsersAndRemoveOnce()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var mockRepository = new EfDeletableEntityRepository<FavoritePost>(new ApplicationDbContext(options.Options));
            var service = new FavoritesService(mockRepository);

            await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", "0a9ec75c-0560-4e5b-94d4-c44429bb7379");
            await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", "65c47bf7-18e6-423b-adee-6c9dbe71a382");
            await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", "65c47bf7-18e6-423b-adee-6c9dbe71a382");
            var postFavouritesCount = service.GetCount("d3946347-0005-45e0-8a02-ad8179d2ece6");

            Assert.Equal(1, postFavouritesCount);
        }
        [Fact]
        public async Task AddOneHundredTimesToFavouritesFromOneHundredDifferentUsers()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var mockRepository = new EfDeletableEntityRepository<FavoritePost>(new ApplicationDbContext(options.Options));
            var service = new FavoritesService(mockRepository);

            for (int i = 1; i <= 100; i++)
            {
                await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", Guid.NewGuid().ToString());

            }
            var postFavouritesCount = service.GetCount("d3946347-0005-45e0-8a02-ad8179d2ece6");

            Assert.Equal(100, postFavouritesCount);
        }
        [Fact]
        public async Task AddOneHundredTimesToFavouritesFromOneHundredDifferentUsersAndRemove99Times()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var mockRepository = new EfDeletableEntityRepository<FavoritePost>(new ApplicationDbContext(options.Options));
            var service = new FavoritesService(mockRepository);

            for (int i = 1; i <= 100; i++)
            {
                var currUser = Guid.NewGuid().ToString();
                await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", currUser);
                await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", currUser);
            }
            await service.FavorAsync("d3946347-0005-45e0-8a02-ad8179d2ece6", Guid.NewGuid().ToString());
            var postFavouritesCount = service.GetCount("d3946347-0005-45e0-8a02-ad8179d2ece6");

            Assert.Equal(1, postFavouritesCount);
        }
    }
}
