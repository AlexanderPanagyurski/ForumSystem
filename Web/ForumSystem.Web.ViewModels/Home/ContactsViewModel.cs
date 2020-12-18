namespace ForumSystem.Web.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    public class ContactsViewModel
    {
        public string Id { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Name { get; set; }

        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Subject { get; set; }

        public string Message { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
