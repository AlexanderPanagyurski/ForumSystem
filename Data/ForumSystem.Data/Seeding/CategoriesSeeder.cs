namespace ForumSystem.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using Microsoft.EntityFrameworkCore.Internal;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new List<string>
            {
                "Sport",
                "Programming",
                "Games",
                "News",
                "Music",
                "Movies",
                "Books",
                "Cats",
                "Dogs",
            };

            foreach (var category in categories)
            {
                await dbContext.Categories.AddAsync(new Category
                {
                    Title = category,
                    Name = category,
                    Description = category,
                });
            }
        }
    }
}
