namespace ForumSystem.Web.ViewModels.Posts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Ganss.XSS;

    public class PostViewModel : IMapFrom<Post>, IMapTo<Post>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserProfileImage { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string UserUserName { get; set; }

        public int VotesCount { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }

        public IEnumerable<PostCommentViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
               .ForMember(x => x.VotesCount, options =>
               {
                   options.MapFrom(p => p.Votes.Sum(v => (int)v.VoteType));
               });
        }
    }
}
