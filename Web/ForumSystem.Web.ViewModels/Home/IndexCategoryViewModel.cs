namespace ForumSystem.Web.ViewModels.Home
{
    public class IndexCategoryViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string Url => $"/c/{this.Name.Replace(' ', '-')}";

        public string ImageUrl { get; set; }

        public int PostsCount { get; set; }
    }
}
