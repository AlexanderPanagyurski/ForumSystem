using ForumSystem.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace ForumSystem.Services.Tests
{
    public class HomePageTests
    {
        [Fact]
        public async Task HomePageShouldHaveTitle()
        {
            var serverFactory = new WebApplicationFactory<Startup>();
            var client = serverFactory.Server.CreateClient();

            var responseMessage=await client.GetAsync("/Home/Index");
            var responseAsString = await responseMessage.Content.ReadAsStringAsync();
            Assert.Contains("<h5>", responseAsString);
        }
    }
}
