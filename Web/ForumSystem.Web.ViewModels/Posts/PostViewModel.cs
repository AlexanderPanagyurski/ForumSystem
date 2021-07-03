namespace ForumSystem.Web.ViewModels.Posts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using AutoMapper;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Ganss.XSS;

    public class PostViewModel : IMapFrom<Post>, IMapTo<Post>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserProfileImage { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

       
        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string UserUserName { get; set; }

        public int VotesCount { get; set; }

        public int FavoritesCount { get; set; }

        public bool IsOwner { get; set; }

        public IEnumerable<PostCommentViewModel> Comments { get; set; }

        public IEnumerable<ImagesViewModel> Images { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
               .ForMember(x => x.VotesCount, options =>
               {
                   options.MapFrom(p => p.Votes.Sum(v => (int)v.VoteType));
               })
               .ForMember(x => x.FavoritesCount, options =>
               {
                   options.MapFrom(p => p.FavoritePosts.Count());
               })
               .ForMember(x => x.Images, options =>
               {
                   options.MapFrom(x => x.Images
                   .Select(x => new ImagesViewModel
                   {
                       ImageUrl = "/images/posts/" + x.Id + "." + x.Extension,
                   }));
               })
               .ForMember(x => x.UserProfileImage, options =>
               {
                   options.MapFrom(x => (x.User.UserImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ?
                   "/images/users/" + x.User.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." + x.User.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : "/images/users/default-profile-icon.jpg");
               })
               ;
        }
    }
}
