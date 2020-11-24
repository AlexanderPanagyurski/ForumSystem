namespace ForumSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.FavoritePosts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoritesService favoritesService;
        private readonly UserManager<ApplicationUser> userManager;

        public FavoritesController(IFavoritesService favoritesService, UserManager<ApplicationUser> userManager)
        {
            this.favoritesService = favoritesService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<FavoritePostResponseModel>> Post(FavoritePostInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.favoritesService.FavorAsync(input.PostId, userId);
            var favoritesCount = this.favoritesService.GetCount(input.PostId);
            return new FavoritePostResponseModel { FavoritesCount = favoritesCount };
        }
    }
}
