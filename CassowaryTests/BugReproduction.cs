using Cassowary_moddiff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cassowary_moddiffTests
{
    [TestClass]
    public class BugReproduction
    {
        /// <summary>
        /// https://github.com/martindevans/Cassowary.net/issues/1
        /// </summary>
        [TestMethod]
        public void EndEdit_NonRequiredVariable_SolvesCorrectly()
        {
            //Create a solver
            var solver = new ClSimplexSolver();
            var variable = new ClVariable(0f);

            //Add a stay, indicating this var should stay at it's current value if possible
            solver.AddStay(variable, ClStrength.Strong, 1);

            //Bug Fixing Note: The stay is strong, which overpowers the SuggestValue. If the stay is Medium or Weak then the SuggestValue takes hold and this test does *not* fail!
            //tl;dr: This bug only happens if the suggest value does nothing?

            const double EXPECTED_VALUE = 10.0;

            //Suggest a value for the variable in an edit context
            var editContext = solver.BeginEdit(variable);
            editContext.SuggestValue(variable, EXPECTED_VALUE);
            editContext.EndEdit();
        }

        /// <summary>
        /// https://github.com/martindevans/Cassowary.net/issues/2
        /// </summary>
        [TestMethod]
        public void EndEdit_Throws()
        {
            var value = new ClVariable("value", 0);

            var solver = new ClSimplexSolver();

            solver.AddStay(value, ClStrength.Strong);

            solver.BeginEdit(value)
                  .SuggestValue(value, 25)
                  .EndEdit(); // <- Exception raised here
        }
    }
}
