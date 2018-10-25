﻿namespace Unosquare.Swan.AspNetCore
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represent the controller of the business rules.
    /// </summary>
    /// <typeparam name="TDbContext">The type of database.</typeparam>
    /// <seealso cref="Unosquare.Swan.AspNetCore.IBusinessRulesController" />
    public abstract class BusinessRulesController<TDbContext> : IBusinessRulesController
        where TDbContext : DbContext
    {
        private readonly MethodInfo[] _methodInfoSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRulesController{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected BusinessRulesController(TDbContext context)
        {
            Context = context;

            _methodInfoSet = GetType()
                .GetMethods()
                .Where(m => (m.ReturnType == typeof(void) || m.ReturnType == typeof(Task))
                            && m.IsPublic
                            && !m.IsConstructor &&
                            m.GetCustomAttributes(typeof(BusinessRuleAttribute),
                                    true)
                                .Any())
                .ToArray();
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public TDbContext Context { get; protected set; }

        /// <summary>
        /// Runs the business rules.
        /// </summary>
        public void RunBusinessRules()
        {
            ExecuteBusinessRulesMethods(EntityState.Added, ActionFlags.Create);
            ExecuteBusinessRulesMethods(EntityState.Modified, ActionFlags.Update);
            ExecuteBusinessRulesMethods(EntityState.Deleted, ActionFlags.Delete);
        }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The type of the current instance.</returns>
        public Type GetEntityType(object entity) => entity.GetType();

        private void ExecuteBusinessRulesMethods(EntityState state, ActionFlags action)
        {
            var selfTrackingEntries = Context.ChangeTracker.Entries()
                .Where(x => x.State == state)
                .Select(x => x.Entity)
                .ToList();

            foreach (var entity in selfTrackingEntries)
            {
                var entityType = entity.GetType();

                var methods = _methodInfoSet
                    .Where(m => m.GetCustomAttributes(typeof(BusinessRuleAttribute), true)
                    .Select(a => a as BusinessRuleAttribute)
                    .Any(b => b != null && (b.EntityTypes == null ||
                                            b.EntityTypes.Any(t => t == entityType)) &&
                              b.Action == action));

                foreach (var methodInfo in methods)
                {
                    methodInfo.Invoke(this, new[] { entity });
                }
            }
        }
    }
}