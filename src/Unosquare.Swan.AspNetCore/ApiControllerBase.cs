using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Swan.AspNetCore
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a API Controller with extended methods related to
    /// the database provide in <c>TDbContext</c>.
    /// </summary>
    /// <typeparam name="TDbContext">The database type.</typeparam>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    public abstract class ApiControllerBase<TDbContext> : ControllerBase
        where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        protected ApiControllerBase(TDbContext context)
        {
            DbContext = context;
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        protected TDbContext DbContext { get; }

        /// <summary>
        /// Retrieve the first element matching the where expression
        /// or not found response code (404).
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="whereExpression">The where expression.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the query database operation.</returns>
        public async Task<IActionResult> FirstOrNotFound<TEntity>(
            Expression<Func<TEntity, bool>> whereExpression,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var current = await DbContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(whereExpression, cancellationToken)
                .ConfigureAwait(false);

            return EntityOrNull(current);
        }

        /// <summary>
        /// Retrieve the first element with the selection expression and
        /// matching the where expression or not found response code (404).
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="whereExpression">The where expression.</param>
        /// <param name="selectExpression">The select expression.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the query database operation.</returns>
        public async Task<IActionResult> FirstOrNotFound<TEntity>(
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> selectExpression,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var current = await DbContext
                .Set<TEntity>()
                .Where(whereExpression)
                .Select(selectExpression)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return EntityOrNull(current);
        }

        /// <summary>
        /// Finds the matching element using the provided keys
        /// or not found response code (404).
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns>A task representing the query database operation.</returns>
        public async Task<IActionResult> Single<TEntity>(
            params object[] keys)
            where TEntity : class
        {
            var current = await DbContext
                .Set<TEntity>()
                .FindAsync(keys)
                .ConfigureAwait(false);

            return EntityOrNull(current);
        }

        /// <summary>
        /// Finds the matching element using the provided keys
        /// or not found response code (404).
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task representing the query database operation.
        /// </returns>
        public async Task<IActionResult> Single<TEntity>(
            object[] keys,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            var current = await DbContext
                .Set<TEntity>()
                .FindAsync(keys, cancellationToken)
                .ConfigureAwait(false);

            return EntityOrNull(current);
        }

        /// <summary>
        /// Queries the specified query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>A task representing the query to the database.</returns>
        public IActionResult Query<TEntity>(
                    Func<IQueryable<TEntity>, IQueryable> query)
                    where TEntity : class
        {
            var set = DbContext.Set<TEntity>();

            return Ok(query(set));
        }

        /// <summary>
        /// Deletes the specified model by the provided keys.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns>A task representing the database operation.</returns>
        public async Task<IActionResult> Delete<TEntity>(
                    params object[] keys)
                    where TEntity : class
        {
            try
            {
                var set = DbContext.Set<TEntity>();
                var entity = await set.FindAsync(keys).ConfigureAwait(false);

                if (entity == null)
                    return NotFound();

                set.Remove(entity);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);

                return Ok();
            }
            catch (TargetInvocationException targetEx)
            {
                throw targetEx.InnerException ?? targetEx;
            }
        }

        /// <summary>
        /// Deletes the specified model by the provided keys.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task representing the database operation.
        /// </returns>
        public async Task<IActionResult> Delete<TEntity>(
            object[] keys,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            try
            {
                var set = DbContext.Set<TEntity>();
                var entity = await set.FindAsync(keys, cancellationToken).ConfigureAwait(false);

                if (entity == null)
                    return NotFound();

                set.Remove(entity);
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Ok();
            }
            catch (TargetInvocationException targetEx)
            {
                throw targetEx.InnerException ?? targetEx;
            }
        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task representing the database operation.
        /// </returns>
        public async Task<IActionResult> Create<TEntity>(
                    TEntity model,
                    CancellationToken cancellationToken = default)
                    where TEntity : class
        {
            try
            {
                DbContext.Set<TEntity>().Add(model);
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Ok();
            }
            catch (TargetInvocationException targetEx)
            {
                throw targetEx.InnerException ?? targetEx;
            }
        }

        /// <summary>
        /// Updates the specified model only with the provide list of properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="includedProperties">The included properties.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>A task representing the write operation to the database.</returns>
        public Task<IActionResult> UpdateOnly<TEntity>(
            TEntity model,
            string[] includedProperties,
            params object[] keys)
            where TEntity : class
            => UpdateEntity(model, true, includedProperties, keys, default);

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="ignoredProperties">The ignored properties.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>A task representing the write operation to the database.</returns>
        public Task<IActionResult> UpdateWithIgnoredProperties<TEntity>(
            TEntity model,
            string[] ignoredProperties,
            params object[] keys)
            where TEntity : class
            => UpdateEntity(model, false, ignoredProperties, keys, default);

        /// <summary>
        /// Updates the specified model. If the entity class contains
        /// the <c>Copyable</c> attribute will copy only the tagged properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>A task representing the write operation to the database.</returns>
        public Task<IActionResult> Update<TEntity>(
            TEntity model,
            params object[] keys)
            where TEntity : class
        {
            var propertiesToCopy = model.GetCopyableProperties().ToArray();

            return propertiesToCopy.Any()
                ? UpdateOnly(model, propertiesToCopy, keys)
                : Update(model, keys, default);
        }

        /// <summary>
        /// Updates the specified model only with the provide list of properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="includedProperties">The included properties.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task representing the write operation to the database.
        /// </returns>
        public Task<IActionResult> UpdateOnly<TEntity>(
            TEntity model,
            string[] includedProperties,
            object[] keys,
            CancellationToken cancellationToken)
            where TEntity : class
            => UpdateEntity(model, true, includedProperties, keys, cancellationToken);

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="ignoredProperties">The ignored properties.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task representing the write operation to the database.
        /// </returns>
        public Task<IActionResult> UpdateWithIgnoredProperties<TEntity>(
            TEntity model,
            string[] ignoredProperties,
            object[] keys,
            CancellationToken cancellationToken)
            where TEntity : class
            => UpdateEntity(model, false, ignoredProperties, keys, cancellationToken);

        /// <summary>
        /// Updates the specified model. If the entity class contains
        /// the <c>Copyable</c> attribute will copy only the tagged properties.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task representing the write operation to the database.
        /// </returns>
        public Task<IActionResult> Update<TEntity>(
            TEntity model,
            object[] keys,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            var propertiesToCopy = model.GetCopyableProperties().ToArray();

            return propertiesToCopy.Any()
                ? UpdateOnly(model, propertiesToCopy, keys, cancellationToken)
                : UpdateEntity(model, false, propertiesToCopy, keys, cancellationToken);
        }

        private async Task<IActionResult> UpdateEntity<TEntity>(
            TEntity model,
            bool only,
            string[] properties,
            object[] keys,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            if (model == null)
                return BadRequest();

            var set = DbContext.Set<TEntity>();
            var entity = await set.FindAsync(keys).ConfigureAwait(false);

            if (entity == null)
                return NotFound();

            if (only)
                model.CopyOnlyPropertiesTo(entity, properties);
            else
                model.CopyPropertiesTo(entity, properties);

            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Ok();
        }

        private IActionResult EntityOrNull(object model) => model == null ? (IActionResult) NotFound() : Ok(model);
    }
}