namespace ForumSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ChatController : BaseController
    {
        public IActionResult All()
        {
            return this.View();
        }
    }
}
