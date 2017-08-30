[![Build Status](https://travis-ci.org/unosquare/swan-aspnetcore.svg?branch=master)](https://travis-ci.org/unosquare/swan-aspnetcore)
[![Build status](https://ci.appveyor.com/api/projects/status/q408tg5jd9bm0jak/branch/master?svg=true)](https://ci.appveyor.com/project/geoperez/swan-aspnetcore/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/unosquare/swan-aspnetcore/badge.svg?branch=master)](https://coveralls.io/github/unosquare/swan-aspnetcore?branch=master)
# <img src="https://github.com/unosquare/swan/raw/master/swan-logo-32.png"></img> SWAN: Stuff We All Need

*:star: Please star this project if you find it useful!*

# Swan ASP.NET Core 2

A set of libraries to use with ASP.NET Core 2.0 applications. Also, includes a configure middleware and extension to setup your project. ASP.NET Core 2.0 came with a lot of changes, including authentication and authorization, here with Swan ASP.NET Core 2.0 is easy to configure and start working on your project.

NuGet Installation:
-------------------

[![NuGet version](https://badge.fury.io/nu/Unosquare.Swan.AspNetCore.svg)](https://badge.fury.io/nu/Unosquare.AspNetCore.Swan)

```
PM> Install-Package Unosquare.Swan.AspNetCore
```

## Use Cases

Here we can find very useful code to use in our project configuration. All of this you can use it in your Startup.cs file, see our [Sample Project](https://github.com/unosquare/swan-aspnetcore/tree/master/src/Unosquare.Swan.AspNetCore.Sample) for more reference.

### Add BearerTokenAuthentication and Use BearerTokenAuthentication

Add BearerTokenAuthentication adds the services that are going to be used in your application. This method uses the Jwt schemes and adds a JwtBearer with the Token Validation Parameters that you configure. Jwt stands for [JSON Web Tokens](https://jwt.io/introduction/)

Use BearerTokenAuthentication is important because it gives the application the requirements to use authentication and authorization with JWT.

With this configuration, you just need to add the data annotation [Authorize] to your API to say that the user needs to be authorized to access that part of your project.

This two are used together because you need to add the bearer token authentication to the services to use the bearer token authentication in your application. You just need to added in your `ConfigureServices` and your `Configure`.

```csharp
// This method gets called by the runtime. Use this method to add services to the container
public void ConfigureServices(IServiceCollection services)
{
    // Extension method to add Bearer authentication
    services.AddBearerTokenAuthentication(ValidationParameters);
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    // Use the bearer token provider and check Admin and Pass.word as valid credentials
    app.UseBearerTokenAuthentication(
        ValidationParameters,
        (username, password, grantType, clientId) =>
        {
            if (username != "Admin" || password != "Pass.word")
                return Task.FromResult<ClaimsIdentity>(null);

            var claim = new ClaimsIdentity("Bearer");
            claim.AddClaim(new Claim(ClaimTypes.Name, username));

            return Task.FromResult(claim);
        }, (identity, obj) =>
        {
            // This action is optional
            obj["test"] = "OK";
            return Task.FromResult(obj);
        });
}
```

### EntityFrameworkLogger

Represents a Logger using EntityFramework. Adds an entity framework logger. It will help you log to your Database everything necessary to know what’s going on in your application. This logger is used in the `Configure` method of your Startup.cs file.

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    // Log levels set in your configuration
    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
    // Here we add the entity framework with the database and the model to utilize
    loggerFactory.AddEntityFramework<SampleDbContext, Models.LogEntry>(app.ApplicationServices);
}
```

### AuditTrail

[Audit Trails](https://github.com/unosquare/ef-enterpriseextensions) is a task to save the changes to any operation perform in a record. In other words, capture what change between any data saving. This operation is important in many system and you can accomplish with these extensions easily. The AuditTrailController can be attached to your BusinessDbContext and setup which Entities will be recorded in the three CRUD actions supported, create, update and delete.

```csharp
public class SampleDbContext : BusinessDbContext
    {
         public SampleDbContext(DbContextOptions<SampleDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            var auditController = new AuditTrailController<SampleDbContext, AuditTrailEntry>(this,
                httpContextAccessor?.HttpContext?.User?.Identity?.Name);
            auditController.AddTypes(ActionFlags.Create, new[] {typeof(Product)});

            AddController(auditController);
        }

        public DbSet<AuditTrailEntry> AuditTrailEntries { get; set; }
    }
```

### UseJsonExceptionHandler

It's very useful to see exceptions in JSON format. You can use this extension to add a very good way to debug your application, you just need to add this to your application builder in the Configure method of your Startup.cs

```csharp
// Response an exception as JSON at error
app.UseJsonExceptionHandler();
```

### UseFallback

Uses the fallback to redirect everything without extension. When the application encountered something without an extension this will help to redirect to the index page or where ever you define to.

```csharp
// Redirect anything without extension to index.html
app.UseFallback();
```