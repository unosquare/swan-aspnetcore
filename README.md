[![Build Status](https://travis-ci.org/unosquare/swan-aspnetcore.svg?branch=master)](https://travis-ci.org/unosquare/swan-aspnetcore)
[![Build status](https://ci.appveyor.com/api/projects/status/q408tg5jd9bm0jak/branch/master?svg=true)](https://ci.appveyor.com/project/geoperez/swan-aspnetcore/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/unosquare/swan-aspnetcore/badge.svg?branch=master)](https://coveralls.io/github/unosquare/swan-aspnetcore?branch=master)

# <img src="https://github.com/unosquare/swan/raw/master/swan-logo-32.png"></img> SWAN: Stuff We All Need

*:star: Please star this project if you find it useful!*

SWAN stands for Stuff We All Need

Repeating code and reinventing the wheel is generally considered bad practice. At Unosquare we are committed to beautiful code and great software. 
Swan is a collection of classes and extension methods that we and other good developers have developed and evolved over the years. We found ourselves copying and pasting 
the same code for every project every time we started it. We decide to kill that cycle once and for all. This is the result of that idea.
Our philosophy is that SWAN should have no external dependencies, it should be cross-platform, and it should be useful.

NuGet Installation:
-------------------

[![NuGet version](https://badge.fury.io/nu/Unosquare.Swan.AspNetCore.svg)](https://badge.fury.io/nu/Unosquare.AspNetCore.Swan)

```
PM> Install-Package Unosquare.Swan.AspNetCore
```

# Swan ASP.NET Core 2

A set of libraries to use with Net Core 2 applications. Also, includes a configure middleware and extension to setup your project. Net core 2 came with a lot of changes, including authentication and authorization, here with Swan Asp Net Core 2 is easy to configure and start working on your project.

## The `Extension` class

Here we can find very useful code to use in our project configuration. All of this you can use it in your Startup.cs file, see our [Sample Project](https://github.com/unosquare/swan-aspnetcore/tree/master/src/Unosquare.Swan.AspNetCore.Sample) for more reference.

#### Example 0: Validation Parameters

It's use to verify the parameters of the authentication. Is use in the `UseBearerTokenAuthentication` and `AddBearerTokenAuthentication`

```csharp
ValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    // You can modify this key in the appsettings.json
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["SymmetricSecurityKey"])),

    ValidateIssuer = true,
    ValidIssuer = "ValidIssuer",

    ValidateAudience = true,
    ValidAudience = "ValidAudience",

    ValidateLifetime = true,

    ClockSkew = TimeSpan.Zero
};
```
### In your `ConfigureServices` method

In this method, you can configure the services of your application.

```csharp
// This method gets called by the runtime. Use this method to add services to the container
public void ConfigureServices(IServiceCollection services)
{
    ...
}
```

#### Example 1: The `AddBearerTokenAuthentication` 

This is required to use the Authentication that comes with core 2 applications.

```csharp
// Extension method to add Bearer authentication
services.AddBearerTokenAuthentication(ValidationParameters);
```

### In your `Configure` method

Here is where you configure your app

```csharp
 // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    ...
}
```

#### Example 1: The `AddEntityFramework`

Adds an entity framework logger. This logger is used in the `Configure` method of your Startup.cs file.

```csharp
// you need to declare as parameter of type ILoggerFactory
loggerFactory.AddEntityFramework<SampleDbContext, Models.LogEntry>(app.ApplicationServices);
```

#### Example 2: The `UseJsonExceptionHandler`

Is very useful to see exceptions in JSON format. 

```csharp
// Response an exception as JSON at error
app.UseJsonExceptionHandler();
```

#### Example 3: The `UseBearerTokenAuthentication`

This is important because it gives the application to use authentication and authorization with JWT (JSON Web tokens). See Example 0 for the ValidationParameters.

With this configuration you just need to add the data annotation [Authorize] to your API to say that the user needs to be authorized to access that part of your project.

```csharp
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
```

#### Example 4: The `UseFallback`

Uses the fallback to redirect everything without extension.

```csharp
// Redirect anything without extension to index.html
app.UseFallback();
```

#### Example 5: The `UseAuditTrail`

This is an extension method to add AuditTrail to a DbContext.

```csharp
// TODO: Example
```