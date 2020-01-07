namespace Swan.AspNetCore.Test
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.IdentityModel.Tokens;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class BearerTokenAuthTest
    {
        private readonly HttpClient _client;
        private readonly TokenValidationParameters _validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SETUPAVALIDSECURITYKEYHERE")),

            ValidateIssuer = true,
            ValidIssuer = "IdentityCore",

            ValidateAudience = true,
            ValidAudience = "Unosquare",

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };

        public BearerTokenAuthTest()
        {
            var server = new TestServer(
                new WebHostBuilder()
                    .Configure(app =>
                    {
                        app.UseBearerTokenAuthentication(
                            _validationParameters,
                            (services, username, password, grantType, clientId) =>
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
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddBearerTokenAuthentication(_validationParameters);
                    }))
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _client = server.CreateClient();
        }

        [Test]
        public async Task TokenTest()
        {
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username","Admin"),
                new KeyValuePair<string, string>("password","Pass.word"),
                new KeyValuePair<string, string>("grant_type","password"),
            };

            var httpMessage = new HttpRequestMessage(HttpMethod.Post, "/api/token");
            httpMessage.Headers.Add("Accept", "application/x-www-form-urlencoded");
            httpMessage.Content = new FormUrlEncodedContent(body);

            var response = await _client.SendAsync(httpMessage);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(responseString.Contains("access_token"));
        }

        [Test]
        public async Task UnauthorizedTokenTest()
        {
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username","Admin"),
                new KeyValuePair<string, string>("password","pass.word"),
                new KeyValuePair<string, string>("grant_type","password"),
            };

            var httpMessage = new HttpRequestMessage(HttpMethod.Post, "/api/token");
            httpMessage.Headers.Add("Accept", "application/x-www-form-urlencoded");
            httpMessage.Content = new FormUrlEncodedContent(body);

            var response = await _client.SendAsync(httpMessage);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "Wrong password");
            Assert.IsTrue(responseString.Contains("Invalid username or password."));
        }

        [Test]
        public async Task InvalidRefreshTokenTest()
        {
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username","Admin"),
                new KeyValuePair<string, string>("password","Pass.word"),
                new KeyValuePair<string, string>("grant_type","refresh_token"),
            };

            var httpMessage = new HttpRequestMessage(HttpMethod.Post, "/api/token");
            httpMessage.Headers.Add("Accept", "application/x-www-form-urlencoded");
            httpMessage.Content = new FormUrlEncodedContent(body);

            var response = await _client.SendAsync(httpMessage);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "Invalid refresh token.");
            Assert.IsTrue(responseString.Contains("Invalid refresh token."));
        }
    }
}
