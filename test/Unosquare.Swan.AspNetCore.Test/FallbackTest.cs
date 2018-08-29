namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Mocks.StartupMocks;
    using NUnit.Framework;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class FallbackTest
    {
        private readonly HttpClient _client;

        public FallbackTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<StartupFallbackMock>());
            _client = server.CreateClient();
        }

        [Test]
        public async Task FallbackRootPathTest()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Root!!", responseString);
        }

        [Test]
        public async Task FallbackProductsRouteTest()
        {
            var response = await _client.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Products!!", responseString);
        }

        [Test]
        public async Task FallbackValuesRouteTest()
        {
            var response = await _client.GetAsync("/fallback");

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("Root!!", responseString, "Redirect to the fallback");
        }
    }
}
