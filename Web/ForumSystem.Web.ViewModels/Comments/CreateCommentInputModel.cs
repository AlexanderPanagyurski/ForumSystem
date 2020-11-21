namespace ForumSystem.Web.ViewModels.Comments
{
    public class CreateCommentInputModel
    {
        public string PostId { get; set; }

        public string Content { get; set; }

        public string ParentId { get; set; }
    }
}
