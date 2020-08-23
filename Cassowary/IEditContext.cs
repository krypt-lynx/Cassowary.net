
namespace Cassowary
{
    /// <summary>
    /// Edit transaction interface
    /// </summary>
    public interface IEditContext
    {
        /// <summary>
        /// Marks the end of an edit session.
        /// </summary>
        /// <remarks>
        /// EndEdit should be called after editing has finished for now, it
        /// just removes all edit variables.
        /// </remarks>
        ClSimplexSolver EndEdit();

        /// <summary>
        /// Suggest a new value for an edit variable. 
        /// </summary>
        /// <remarks>
        /// The variable needs to be added as an edit variable and 
        /// BeginEdit() needs to be called before this is called.
        /// The tableau will not be solved completely until after Resolve()
        /// has been called.
        /// </remarks>
        IEditContext SuggestValue(ClVariable clVariable, double value);

        /// <summary>
        /// Re-solve the current collection of constraints, given the new
        /// values for the edit variables that have already been
        /// suggested (see <see cref="IEditContext.SuggestValue"/> method).
        /// </summary>
        IEditContext Resolve();
    }
}
