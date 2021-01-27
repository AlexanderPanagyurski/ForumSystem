namespace ForumSystem.Data.Models
{
    using System;

    using ForumSystem.Data.Common.Models;

    public class UserImage : BaseDeletableModel<string>
    {
        public UserImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string Extension { get; set; }

        public bool IsProfileImage { get; set; }
    }
}
