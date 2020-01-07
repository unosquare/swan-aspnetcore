using Swan.AspNetCore;
using Swan.AspNetCore.Models;

namespace Swan.AspNetCore.Sample
{
    using Database;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;

            ValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["SymmetricSecurityKey"])),

                ValidateIssuer = true,
                ValidIssuer = "IdentityCore",

                ValidateAudience = true,
                ValidAudience = "Unosquare",

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
        }

        public IConfiguration Configuration { get; }

        private TokenValidationParameters ValidationParameters { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<SampleDbContext>(options => options.UseSqlServer(Configuration["ConnectionString"]));

            // Add Entity Framework Logging Provider
            services.AddLoggingEntityFramework<SampleDbContext, LogEntry>();

            // Extension method to add Bearer authentication
            services.AddBearerTokenAuthentication(ValidationParameters);
            
            // Add framework services.
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app)
        {
            // Redirect anything without extension to index.html
            app.UseFallback();
            // Response an exception as JSON at error
            app.UseJsonExceptionHandler();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Use the bearer token provider and check Admin and Pass.word as valid credentials
            app.UseBearerTokenAuthentication(
                ValidationParameters,
                (services, username, password, grantType, clientId) =>
                {
                    var context = services.GetService<SampleDbContext>();

                    if (context != null && username != "Admin" || password != "Pass.word")
                        return Task.FromResult<ClaimsIdentity?>(null);

                    var claim = new ClaimsIdentity("Bearer");
                    claim.AddClaim(new Claim(ClaimTypes.Name, username));

                    return Task.FromResult(claim);
                }, (identity, obj) =>
                {
                    // This action is optional
                    obj["test"] = "OK";
                    return Task.FromResult(obj);
                }, forceHttps: false);
            
            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });          
        }
    }   
}