﻿namespace Swan.AspNetCore.Test.Mocks.StartupMocks
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System.Net;

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
                    case "/":
                        await context.Response.WriteAsync("Root!!");
                        break;
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                }
            });
        }

        public void ConfigureService(IServiceCollection services)
        {
        }
    }
}
