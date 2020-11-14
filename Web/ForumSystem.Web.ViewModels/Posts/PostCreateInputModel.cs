﻿namespace ForumSystem.Web.ViewModels.Posts
{
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class PostCreateInputModel : IMapTo<Post>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryId { get; set; }

        // public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }
    }
}
