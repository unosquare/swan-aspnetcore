namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    class StartupEFLoggerMock
    {
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddEntityFramework<BusinessDbContextMock, Models.LogEntry>(app.ApplicationServices);
        }

        public void ConfigureService(IServiceCollection services)
        {
        }        
    }
}
