using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Swan.AspNetCore
{
    /// <summary>
    /// Represents a abstract class to create DbContext using Business Rules.
    /// </summary>
    /// <seealso cref="DbContext" />
    /// <seealso cref="IBusinessDbContext" />
    public abstract class BusinessDbContext : DbContext, IBusinessDbContext
    {
        private readonly List<IBusinessRulesController> _businessControllers = new List<IBusinessRulesController>();
        private readonly AutoResetEvent resetEvent = new AutoResetEvent(true);

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessDbContext"/> class.
        /// </summary>
        /// <param name="options">The database connection.</param>
        protected BusinessDbContext(DbContextOptions options)
            : base(options)
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
            resetEvent.WaitOne();

            try
            {
                foreach (var controller in _businessControllers)
                {
                    controller.RunBusinessRules();
                }
            }
            finally
            {
                resetEvent.Set();
            }
        }

        /// <inheritdoc />
        public override int SaveChanges()
        {
            RunBusinessRules();
            return base.SaveChanges();
        }

        /// <inheritdoc />
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            RunBusinessRules();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}