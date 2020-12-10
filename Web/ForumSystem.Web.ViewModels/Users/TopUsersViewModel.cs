namespace ForumSystem.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class TopUsersViewModel
    {
        public IEnumerable<UserViewModel> TopUsers { get; set; }
    }
}
