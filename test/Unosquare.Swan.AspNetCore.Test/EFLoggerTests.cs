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
    using Mocks;
    using Microsoft.AspNetCore.Builder;

    [TestFixture]
    public class EfLoggerTests
    {
        private readonly HttpClient _client;
        
        public EfLoggerTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .Configure(app =>
                {
                    app.ApplicationServices.GetService<ILoggerFactory>()
                        .AddEntityFramework<BusinessDbContextMock, Models.LogEntry>(app.ApplicationServices);

                    app.Run(async (context) =>
                    {
                        await Task.Delay(150);
                        await context.Response.WriteAsync(
                            app.ApplicationServices.GetService<BusinessDbContextMock>().Set< Models.LogEntry>().Count().ToString());
                    });
                })
                .ConfigureServices(services =>
                {
                    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    services
                        .AddEntityFrameworkInMemoryDatabase()
                        .AddDbContext<BusinessDbContextMock>(options =>
                        {
                            options.UseInMemoryDatabase(nameof(EfLoggerTests));
                        });
                }));

            _client = server.CreateClient();
        }

        [Test]
        public async Task EfLoggerDbTest()
        {
            var data = await _client.GetStringAsync("/");
            Assert.Greater(int.Parse(data), 0);
        }
    }
}
