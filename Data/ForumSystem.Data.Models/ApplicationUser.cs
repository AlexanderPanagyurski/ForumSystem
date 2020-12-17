// ReSharper disable VirtualMemberCallInConstructor
namespace ForumSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    using ForumSystem.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        public string ProfileImage { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public string WebsiteUrl { get; set; }

        public string GithubUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string InstagramUrl { get; set; }

        public string FacebookUrl { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<FavoritePost> FavoritePosts { get; set; } = new HashSet<FavoritePost>();

        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();

        public virtual ICollection<UserImage> UserImages { get; set; } = new HashSet<UserImage>();
    }
}
