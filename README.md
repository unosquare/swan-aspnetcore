[![Build Status](https://travis-ci.org/unosquare/swan-aspnetcore.svg?branch=master)](https://travis-ci.org/unosquare/swan-aspnetcore)
[![Build status](https://ci.appveyor.com/api/projects/status/q408tg5jd9bm0jak/branch/master?svg=true)](https://ci.appveyor.com/project/geoperez/swan-aspnetcore/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/unosquare/swan-aspnetcore/badge.svg?branch=master)](https://coveralls.io/github/unosquare/swan-aspnetcore?branch=master)
# <img src="https://github.com/unosquare/swan/raw/master/swan-logo-32.png"></img> Swan ASP.NET Core 2: Stuff We All Need

*:star: Please star this project if you find it useful!*

A set of libraries to use with ASP.NET Core 2.0 applications. Also, includes a configure middleware and extension to setup your project. ASP.NET Core 2.0 came with a lot of changes, including authentication and authorization, here with Swan ASP.NET Core 2.0 is easy to configure and start working on your project.

NuGet Installation:
-------------------

[![NuGet version](https://badge.fury.io/nu/Unosquare.Swan.AspNetCore.svg)](https://badge.fury.io/nu/Unosquare.Swan.AspNetCore)

```
PM> Install-Package Unosquare.Swan.AspNetCore
```

## Use Cases

Here we can find a very useful code to use in our project configuration. All of this you can use it in your Startup.cs file, see our [Sample Project](https://github.com/unosquare/swan-aspnetcore/tree/master/src/Unosquare.Swan.AspNetCore.Sample) for more reference.

### Using Bearer Token Authentication

The extension method `AddBearerTokenAuthentication` adds the services that are going to be used in your application. This method uses the Jwt schemes and adds a JwtBearer with the Token Validation Parameters that you configure. Jwt stands for [JSON Web Tokens](https://jwt.io/introduction/)

The extension method `UseBearerTokenAuthentication` is important because it gives the application the requirements to use authentication and authorization with JWT.

With this configuration, you just need to add the data annotation `[Authorize]` to your API to say that the user needs to be authorized to access that part of your project.

This two are used together because you need to add the bearer token authentication to the services to use the bearer token authentication in your application. You just need to add in your `ConfigureServices` and your `Configure`.

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

### Using EntityFrameworkLogger

The `EntityFrameworkLogger` represents a Logger based on Entity Framework and adds a Logging subsystem. It will help you log into your Database everything necessary to know what’s going on in your application. This logger is used in the `ConfigureServices` method of your Startup.cs file.
With this you just add your configuration section and then add in the entity framework, your database context and the models that you use for log entries into your database, then you just pass the application services.

```csharp
public void ConfigureServices(IServiceCollection services)
{
	// Inject your DbContext first.

    // Add Entity Framework Logging Provider
	services.AddLoggingEntityFramework<SampleDbContext, Models.LogEntry>();
}
```

### Using BusinessDbContext

The BusinessDbContext run business rules when you save changes to the database. It's helpful because if you want to modify an entity every time you save changes to the database, you can create a controller to do that. Your Db Context must inheritance from the BusinessDbContext in order to have this functionality and then you can add, remove and check for controllers using your context.

```csharp
 public class SampleDbContext : BusinessDbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {
        }
    }
```

### AuditTrail

[Audit Trails](https://github.com/unosquare/ef-enterpriseextensions) is a business rule to save the changes to any operation performed in a record. In other words, capture what change between any data saving. This operation is important in many systems and you can accomplish with these extensions easily. The AuditTrailController can be attached to your BusinessDbContext and setup which Entities will be recorded in the three CRUD actions supported, create, update and delete.

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
### Additional Extension Methods

#### The JsonExceptionHandler

It's very useful to see exceptions in JSON format. You can use this extension to add a very good way to debug your application, you just need to add this to your application builder in the Configure method of your Startup.cs.
When an error occurs, the exception handles the error and send a response with a 500-status code, or any other according to the exception type.

```csharp
// Response an exception as JSON at error
app.UseJsonExceptionHandler();
```

#### The Fallback

Uses the fallback to redirect everything without extension. When the application encountered something without an extension this will help to redirect to the index page or where ever you define too. It's like when a URL that is not handled it will automatically redirect to the fallback, the default is index.html.

```csharp
// Redirect anything without extension to index.html
app.UseFallback();
```
