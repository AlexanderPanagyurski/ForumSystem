using ForumSystem.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ForumSystem.Services.Tests
{
    public class HomePageTests
    {
        private readonly ITestOutputHelper outputHelper;

        public HomePageTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public async Task HomePageShouldHaveTitle()
        {
            var serverFactory = new WebApplicationFactory<Startup>();
            var client = serverFactory.Server.CreateClient();

            Stopwatch sw = Stopwatch.StartNew();
            var responseMessage=await client.GetAsync("/Home/Index");
            var responseAsString = await responseMessage.Content.ReadAsStringAsync();
            outputHelper.WriteLine(sw.Elapsed.ToString());
            if (sw.Elapsed > new TimeSpan(0, 0, 1))
            {
                throw new Exception("Too long");
            }
            Assert.Contains("<h5>", responseAsString);
        }
    }
}
