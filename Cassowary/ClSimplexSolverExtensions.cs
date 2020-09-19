using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// this file is a fine example of stackoverflow programming

namespace Cassowary_moddiff
{
    /// <summary>
    /// Constraints builder extension
    /// </summary>
    public static class ConstraintsBuilder
    {
        /// <summary>
        /// Add multiple constraints to the solver.
        /// <param name="solver">
        /// the solver
        /// </param>
        /// <param name="constraints">
        /// The constraint to be added.
        /// </param>
        /// <param name="strength">
        /// strength for the constraints
        /// </param>
        /// </summary>
        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, ClStrength strength, IEnumerable<ClConstraint> constraints)
        {
            System.Diagnostics.Debug.Assert(strength != null, "strength in null");
            foreach (var constraint in constraints)
            {
                constraint.SetStrength(strength);
                solver.AddConstraint(constraint);
            }
            return solver;
        }

        /// <summary>
        /// Add multiple constraints to the solver.
        /// <param name="solver">
        /// the solver
        /// </param>
        /// <param name="constraints">
        /// The constraint to be added.
        /// </param>
        /// <param name="strength">
        /// strength for the constraints
        /// </param>
        /// </summary>
        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, ClStrength strength, params ClConstraint[] constraints)
        {
            return AddConstraints(solver, strength, (IEnumerable<ClConstraint>)constraints);
        }

        /// <summary>
        /// Add multiple constraints to the solver.
        /// <param name="solver">
        /// the solver
        /// </param>
        /// <param name="constraints">
        /// The constraint to be added.
        /// </param>
        /// </summary>
        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, IEnumerable<ClConstraint> constraints)
        {
            foreach (var constraint in constraints)
            {
                solver.AddConstraint(constraint);
            }
            return solver;
        }

        /// <summary>
        /// Add multiple constraints to the solver.
        /// <param name="solver">
        /// the solver
        /// </param>
        /// <param name="constraints">
        /// The constraint to be added.
        /// </param>
        /// </summary>
        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, params ClConstraint[] constraints)
        {
            return AddConstraints(solver, (IEnumerable<ClConstraint>)constraints);
        }

        /// <summary>
        /// Add a constraint to the solver.
        /// <param name="solver">
        /// the solver
        /// </param>
        /// <param name="constraint">
        /// The constraint to be added.
        /// </param>
        /// <param name="strength">
        /// strength for the constraint
        /// </param>
        /// </summary>
        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClConstraint constraint, ClStrength strength)
        {
            System.Diagnostics.Debug.Assert(strength != null, "strength in null");
            constraint.SetStrength(strength);
            return solver.AddConstraint(constraint);
        }
    }
}
