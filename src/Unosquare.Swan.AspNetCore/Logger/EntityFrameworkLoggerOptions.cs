using System.Collections.Generic;

namespace Swan.AspNetCore.Logger
{
    /// <summary>
    /// Represents the EF Logger options.
    /// </summary>
    public class EntityFrameworkLoggerOptions
    {
        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        public IDictionary<string, Microsoft.Extensions.Logging.LogLevel> Filters { get; set; }
    }
}