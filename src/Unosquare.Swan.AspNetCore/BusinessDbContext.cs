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

        /// <inheritdoc />
        public override int SaveChanges()
        {
            RunBusinessRules();
            return base.SaveChanges();
        }

        /// <inheritdoc />
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RunBusinessRules();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}