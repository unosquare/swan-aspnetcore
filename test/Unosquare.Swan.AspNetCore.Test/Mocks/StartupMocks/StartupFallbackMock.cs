
namespace Unosquare.Swan.AspNetCore.Test.Mocks.StartupMocks
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    class StartupFallbackMock
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseFallback("/");

            app.Run(async (context) =>
            {
                switch (context.Request.Path)
                {
                    case "/api/products":
                        await context.Response.WriteAsync("Products!!");
                        break;
                    default:
                        await context.Response.WriteAsync("Fallback!!");
                        break;
                }
            });
        }

        public void ConfigureService(IServiceCollection services)
        {
        }
    }
}
