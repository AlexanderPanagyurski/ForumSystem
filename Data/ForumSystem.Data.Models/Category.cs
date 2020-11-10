namespace ForumSystem.Data.Models
{
    using System.Collections.Generic;

    using ForumSystem.Data.Common.Models;

    public class Category : BaseDeletableModel<string>
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
