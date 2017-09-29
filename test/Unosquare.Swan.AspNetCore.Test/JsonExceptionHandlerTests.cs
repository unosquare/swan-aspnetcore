namespace Unosquare.Swan.AspNetCore.Sample
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Formatters;
    using NUnit.Framework;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;


    public class JsonExceptionHandlerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public class JsonError
        {
            public string Message { get; set; }
        }

        public JsonExceptionHandlerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseJsonExceptionHandler();

                    app.Run((context) => throw new System.Exception("Test Exception"));
                }));
            _client = _server.CreateClient();
        }

        [Test]
        public async Task JsonExceptionHandlerTest()
        {
            var response = await _client.GetAsync("/");

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonError = Json.Deserialize<JsonError>(responseString);
            
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("Test Exception", jsonError.Message);
        }
    }
}
