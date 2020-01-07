using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swan.AspNetCore.Models;

namespace Swan.AspNetCore.Logger
{
    /// <summary>
    /// Represents a EF logger provider.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TLog">The type of the log.</typeparam>
    /// <seealso cref="ILoggerProvider" />
    public sealed class EntityFrameworkLoggerProvider<TDbContext, TLog> : ILoggerProvider
        where TLog : LogEntry, new()
        where TDbContext : DbContext
    {
        private readonly Func<string, Microsoft.Extensions.Logging.LogLevel, bool>? _filter;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkLoggerProvider{TDbContext, TLog}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="filter">The filter.</param>
        public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, Func<string, Microsoft.Extensions.Logging.LogLevel, bool>? filter)
        {
            _filter = filter;
            _serviceProvider = serviceProvider;
        }
        
        /// <inheritdoc />
        public ILogger CreateLogger(string name)
            => new EntityFrameworkLogger<TDbContext, TLog>(name, _filter, _serviceProvider);

        /// <inheritdoc />
        void IDisposable.Dispose()
        {
        }
    }

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
