﻿namespace ForumSystem.Web.ViewModels.Posts
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
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Display(Name = "Category")]
        public string CategoryId { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }
    }
}
