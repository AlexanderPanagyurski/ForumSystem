namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class SearchesController : BaseController
    {
        private readonly ApplicationDbContext db;

        public SearchesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult<string[]> Searches()
        {
            var responseModel = this.db
                .Posts
                .Select(x => x.Title)
                .ToArray();

            return responseModel;
        }
    }
}
