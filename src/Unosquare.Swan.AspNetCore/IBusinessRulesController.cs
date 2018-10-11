namespace Unosquare.Swan.AspNetCore
{
    /// <summary>
    /// Represents a Business Rules Controller.
    /// </summary>
    public interface IBusinessRulesController
    {
        /// <summary>
        /// Runs the business rules.
        /// </summary>
        void RunBusinessRules();
    }
}