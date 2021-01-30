namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data;
    using ForumSystem.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class SearchesController : BaseController
    {
        private readonly ISearchesService searchesService;

        public SearchesController(ISearchesService searchesService)
        {
            this.searchesService = searchesService;
        }

        public ActionResult<string[]> Searches()
        {
            var responseModel = this.searchesService.Searches();

            return responseModel;
        }
    }
}
