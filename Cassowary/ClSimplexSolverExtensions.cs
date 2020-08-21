using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// this file is a fine example of stackoverflow programming

namespace Cassowary
{
    /// <summary>
    /// Constraints builder extension
    /// </summary>
    public static class ConstraintsBuilder
    {
        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, ClStrength strength, IEnumerable<ClConstraint> constraints)
        {
            foreach (var constraint in constraints)
            {
                constraint.SetStrength(strength);
                solver.AddConstraint(constraint);
            }
            return solver;
        }

        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, ClStrength strength, params ClConstraint[] constraints)
        {
            return AddConstraints(solver, strength, (IEnumerable<ClConstraint>)constraints);
        }


        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, IEnumerable<ClConstraint> constraints)
        {
            foreach (var constraint in constraints)
            {
                constraint.SetStrength(ClStrength.Default);
                solver.AddConstraint(constraint);
            }
            return solver;
        }
        
        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, params ClConstraint[] constraints)
        {
            return AddConstraints(solver, (IEnumerable<ClConstraint>)constraints);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClStrength strength, ClConstraint constraint)
        {
            constraint.SetStrength(strength);
            return solver.AddConstraint(constraint);
        }
    }
}
