using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Swan.AspNetCore
{
    /// <summary>
    /// Represents the authenticate scheme to the middleware. 
    /// </summary>
    public class AuthenticateSchemeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _scheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateSchemeMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="scheme">The scheme.</param>
        /// <exception cref="ArgumentNullException">scheme.</exception>
        public AuthenticateSchemeMiddleware(RequestDelegate next, string scheme)
        {
            _next = next;
            _scheme = scheme ?? throw new ArgumentNullException(nameof(scheme));
        }

        /// <summary>
        /// Invokes the specified HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>The HTTP Context with the authenticate scheme.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            var result = await httpContext.AuthenticateAsync(_scheme).ConfigureAwait(false);

            if (result.Succeeded)
            {
                httpContext.User = result.Principal;
            }

            await _next(httpContext).ConfigureAwait(false);
        }
    }
}
