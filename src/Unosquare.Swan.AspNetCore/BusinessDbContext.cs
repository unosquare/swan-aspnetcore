namespace Unosquare.Swan.AspNetCore
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Represents a abstract class to create DbContext using Business Rules.
    /// </summary>
    /// <seealso cref="DbContext" />
    /// <seealso cref="IBusinessDbContext" />
    public abstract class BusinessDbContext : DbContext, IBusinessDbContext
    {
        private readonly List<IBusinessRulesController> _businessControllers = new List<IBusinessRulesController>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessDbContext"/> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        protected BusinessDbContext(DbContextOptions dbConnection)
            : base(dbConnection)
        {
        }

        /// <inheritdoc />
        public void AddController(IBusinessRulesController controller) => _businessControllers.Add(controller);

        /// <inheritdoc />
        public void RemoveController(IBusinessRulesController controller) => _businessControllers.Remove(controller);

        /// <inheritdoc />
        public bool ContainsController(IBusinessRulesController controller) => _businessControllers.Contains(controller);

        /// <summary>
        /// Runs the business rules.
        /// </summary>
        public void RunBusinessRules()
        {
            foreach (var controller in _businessControllers)
            {
                controller.RunBusinessRules();
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </remarks>
        public override int SaveChanges()
        {
            RunBusinessRules();
            return base.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        /// number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </para>
        /// <para>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </para>
        /// </remarks>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RunBusinessRules();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}