namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class PostCreateInputModel : IMapTo<Post>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }
    }
}
