namespace Unosquare.Swan.AspNetCore.Sample
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Swan.Formatters;
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
            public object Data { get; set; }
            public string InnerException { get; set; }
            public string TargetSite { get; set; }
            public string StackTrace { get; set; }
            public string HelpLink { get; set; }
            public string Source { get; set; }
            public string IPForWatsonBuckets { get; set; }
            public string WatsonBuckets { get; set; }
            public string RemoteStackTrace { get; set; }
            public string HResult { get; set; }
            public bool IsTransient { get; set; }

        }

        public JsonExceptionHandlerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseJsonExceptionHandler();

                    app.Run((context) =>
                    {
                        throw new System.Exception("Test Exception");
                    });
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
