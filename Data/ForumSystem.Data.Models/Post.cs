namespace ForumSystem.Data.Models
{
    using System.Collections.Generic;

    using ForumSystem.Data.Common.Models;

    public class Post : BaseDeletableModel<string>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
