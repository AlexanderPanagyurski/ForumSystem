namespace ForumSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using ForumSystem.Data.Common.Models;
    using ForumSystem.Data.Models.Enums;

    public class Vote : BaseModel<int>
    {
        [Required]
        public string PostId { get; set; }

        public virtual Post Post { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public VoteType VoteType { get; set; }
    }
}
