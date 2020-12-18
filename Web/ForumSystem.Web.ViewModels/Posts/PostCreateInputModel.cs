namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class PostCreateInputModel : IMapTo<Post>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Display(Name = "Category")]
        public string CategoryId { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }
    }
}
