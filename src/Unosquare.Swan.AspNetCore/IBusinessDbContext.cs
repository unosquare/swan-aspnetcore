namespace Unosquare.Swan.AspNetCore
{
    /// <summary>
    /// Creates a new DbContext with support to run BusinessControllers.
    /// </summary>
    public interface IBusinessDbContext
    {
        /// <summary>
        /// Adds the controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        void AddController(IBusinessRulesController controller);

        /// <summary>
        /// Removes the controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        void RemoveController(IBusinessRulesController controller);

        /// <summary>
        /// Determines whether the specified controller contains controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns>
        ///   <c>true</c> if the specified controller contains controller; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsController(IBusinessRulesController controller);
    }
}