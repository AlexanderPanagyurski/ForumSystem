namespace ForumSystem.Data
{
    using System;

    using ForumSystem.Data.Common.Models;
    using ForumSystem.Data.Models;

    public class FavoritePost : BaseDeletableModel<string>
    {
        public FavoritePost()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string PostId { get; set; }

        public virtual Post Post { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
