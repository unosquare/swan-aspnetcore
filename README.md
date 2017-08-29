[![Build Status](https://travis-ci.org/unosquare/swan-aspnetcore.svg?branch=master)](https://travis-ci.org/unosquare/swan-aspnetcore)
[![Build status](https://ci.appveyor.com/api/projects/status/q408tg5jd9bm0jak/branch/master?svg=true)](https://ci.appveyor.com/project/geoperez/swan-aspnetcore/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/unosquare/swan-aspnetcore/badge.svg?branch=master)](https://coveralls.io/github/unosquare/swan-aspnetcore?branch=master)

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

### Case 1: Add BearerTokenAuthentication and Use BearerTokenAuthentication

Add BearerTokenAuthentication this only adds services to your project.

Use BearerTokenAuthentication is important because it gives the application the requirements to use authentication and authorization with JWT (JSON Web tokens).

With this configuration, you just need to add the data annotation [Authorize] to your API to say that the user needs to be authorized to access that part of your project.

This two are used together because you need to add the bearer token authentication to the services to use the bearer token authentication. You just need to added in your `ConfigureServices` and your `Configure`.

```csharp
// This method gets called by the runtime. Use this method to add services to the container
public void ConfigureServices(IServiceCollection services)
{
// Extension method to add Bearer authentication
services.AddBearerTokenAuthentication(ValidationParameters);
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
public void Configure(IApplicationBuilder app)
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

### Case 2: EntityFrameworkLogger

Represents a Logger using EntityFramework

#### Example 1: AddEntityFramework

Adds an entity framework logger. This logger is used in the `Configure` method of your Startup.cs file.

```csharp
// you need to declare as parameter of type ILoggerFactory
loggerFactory.AddEntityFramework<SampleDbContext, Models.LogEntry>(app.ApplicationServices);
```

### Case 3: AuditTrail

This is an extension method to add AuditTrail to a DbContext.

```csharp
// TODO: Example
```

### Case 4: UseJsonExceptionHandler

It's very useful to see exceptions in JSON format. 

```csharp
// Response an exception as JSON at error
app.UseJsonExceptionHandler();
```

### Case 5: UseFallback

Uses the fallback to redirect everything without extension.

```csharp
// Redirect anything without extension to index.html
app.UseFallback();
```