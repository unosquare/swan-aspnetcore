namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using NUnit.Framework;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Unosquare.Swan.AspNetCore.Test.Mocks;

    class EFLoggerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private ILoggerFactory loggerFactory;

        private BusinessDbContextMock SetupDatabase(string name)
        {
            var builder = new DbContextOptionsBuilder<BusinessDbContextMock>()
               .UseInMemoryDatabase(name);
            var options = builder.Options;
            return new BusinessDbContextMock(options);
        }

        public EFLoggerTests()
        {
            // TODO: use loggerFactory
            loggerFactory = new LoggerFactory();
            _server = new TestServer(new WebHostBuilder()
                .Configure(app =>
               {
                   loggerFactory.AddEntityFramework<BusinessDbContextMock, Models.LogEntry>(app.ApplicationServices);
               })
                .ConfigureServices( services => 
                {
                    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    services.AddEntityFrameworkInMemoryDatabase();
                }));
            _client = _server.CreateClient();
        }

        [Test]
        public async Task EFLoggerDbTest()
        {
            var response = await _client.GetAsync("/");
            var responseString = response.Content.ReadAsStringAsync();

            using (var context = SetupDatabase(nameof(EFLoggerDbTest)))
            {
                Assert.IsTrue(context.LogEntries.Any());
            }
        }
    }
}
