namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using NUnit.Framework;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Unosquare.Swan.AspNetCore.Test.Mocks;

    class EFLoggerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        private BusinessDbContextMock SetupDatabase(string name)
        {
            var builder = new DbContextOptionsBuilder<BusinessDbContextMock>()
               .UseInMemoryDatabase(name);
            var options = builder.Options;
            return new BusinessDbContextMock(options);
        }

        public EFLoggerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<StartupEFLoggerMock>()
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
