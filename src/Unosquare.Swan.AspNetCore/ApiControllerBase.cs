namespace Unosquare.Swan.AspNetCore
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc />
    /// <summary>
    /// Represents a API Controller with extended methods related to
    /// the database provide in <c>T</c>.
    /// </summary>
    /// <typeparam name="TDbContext">The database type.</typeparam>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    public abstract class ApiControllerBase<TDbContext> : ControllerBase
    where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase{T}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        protected ApiControllerBase(TDbContext dbContext)
        {
            DbContext = dbContext;
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
        public virtual async Task<IActionResult> FirstOrNotFound<TEntity>(
            Expression<Func<TEntity, bool>> whereExpression,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var current = await DbContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(whereExpression, cancellationToken);

            if (current == null)
                return NotFound();

            return Ok(current);
        }

        /// <summary>
        /// Firsts the or not found.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="whereExpression">The where expression.</param>
        /// <param name="selectExpression">The select expression.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the query database operation.</returns>
        public virtual async Task<IActionResult> FirstOrNotFound<TEntity>(
                    Expression<Func<TEntity, bool>> whereExpression,
                    Expression<Func<TEntity, object>> selectExpression,
                    CancellationToken cancellationToken = default)
                    where TEntity : class
        {
            var current = await DbContext
                .Set<TEntity>()
                .Where(whereExpression)
                .Select(selectExpression)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (current == null)
                return NotFound();

            return Ok(current);
        }
    }
}
