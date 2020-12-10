namespace ForumSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ForumSystem.Web.ViewModels.Users;

    public interface IUsersService
    {
        TopUsersViewModel GetTopUsers();
    }
}
