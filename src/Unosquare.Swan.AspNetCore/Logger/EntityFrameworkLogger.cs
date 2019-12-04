using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swan.AspNetCore.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Swan.AspNetCore.Logger
{
    /// <summary>
    /// Represents a Logger using EntityFramework
    /// 
    /// Based on https://github.com/staff0rd/entityframework-logging.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TLog">The type of the log.</typeparam>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public class EntityFrameworkLogger<TDbContext, TLog> : Microsoft.Extensions.Logging.ILogger
        where TLog : LogEntry, new()
        where TDbContext : DbContext
    {
        private readonly string _name;
        private readonly Func<string, Microsoft.Extensions.Logging.LogLevel, bool> _filter;
        private readonly IServiceProvider _services;
        private readonly ConcurrentQueue<TLog> _entryQueue = new ConcurrentQueue<TLog>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkLogger{TDbContext, TLog}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public EntityFrameworkLogger(string name, Func<string, Microsoft.Extensions.Logging.LogLevel, bool>? filter, IServiceProvider serviceProvider)
        {
            _name = name;
            _filter = filter ?? GetFilter(serviceProvider.GetService<IOptions<EntityFrameworkLoggerOptions>>());
            _services = serviceProvider;

            Task.Run(async () =>
            {
                while (true)
                {
                    if (_entryQueue.Count > 0)
                    {
                        try
                        {
                            var db = _services.GetService<TDbContext>();
                            db.ChangeTracker.AutoDetectChangesEnabled = false;
                            while (_entryQueue.Count > 0)
                            {
                                if (_entryQueue.TryDequeue(out var entry))
                                    db.Set<TLog>().Add(entry);
                            }

                            await db.SaveChangesAsync().ConfigureAwait(false);
                        }
                        catch
                        {
                            // Ignored
                        }
                    }

                    await Task.Delay(50).ConfigureAwait(false);
                }

                // ReSharper disable once FunctionNeverReturns
            });
        }

        /// <inheritdoc />
        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => _filter(_name, logLevel);

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => new NoopDisposable();

        /// <inheritdoc />
        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (_name.StartsWith("Microsoft.EntityFrameworkCore") || !IsEnabled(logLevel)) return;

            string message;

            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            else
            {
                message = state.Stringify();

                if (exception != null)
                    message += Environment.NewLine + exception.Stringify();
            }

            if (string.IsNullOrEmpty(message)) return;

            var log = new TLog
            {
                Message = message.Truncate(LogEntry.MaximumMessageLength),
                Date = DateTime.UtcNow,
                Level = logLevel.ToString(),
                Logger = _name,
                Thread = eventId.ToString(),
            };

            if (exception != null)
                log.Exception = exception.ToString().Truncate(LogEntry.MaximumExceptionLength);

            var httpContext = _services.GetRequiredService<IHttpContextAccessor>()?.HttpContext;

            if (httpContext != null)
            {
                log.Browser = httpContext.Request.Headers["User-Agent"];
                log.Username = httpContext.User.Identity.Name;

                try
                {
                    log.HostAddress = httpContext.Connection.LocalIpAddress?.ToString();
                }
                catch (ObjectDisposedException)
                {
                    log.HostAddress = "Disposed";
                }

                log.Url = httpContext.Request.Path;
            }

            _entryQueue.Enqueue(log);
        }

        private static bool GetFilter(EntityFrameworkLoggerOptions options, string category, Microsoft.Extensions.Logging.LogLevel level)
        {
            var filter = options.Filters?.Keys.FirstOrDefault(category.StartsWith);
            return filter == null || (int)options.Filters![filter] <= (int)level;
        }

        private Func<string, Microsoft.Extensions.Logging.LogLevel, bool> GetFilter(IOptions<EntityFrameworkLoggerOptions> options)
        {
            if (options != null)
                return (category, level) => GetFilter(options.Value, category, level);

            return (category, level) => true;
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}