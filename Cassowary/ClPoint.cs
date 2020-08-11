/*
  Cassowary.net: an incremental constraint solver for .NET
  (http://lumumba.uhasselt.be/jo/projects/cassowary.net/)
  
  Copyright (C) 2005-2006  Jo Vermeulen (jo.vermeulen@uhasselt.be)
  
  This program is free software; you can redistribute it and/or
  modify it under the terms of the GNU Lesser General Public License
  as published by the Free Software Foundation; either version 2.1
  of  the License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Lesser General Public License for more details.

  You should have received a copy of the GNU Lesser General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

using System.Collections.Generic;

namespace Cassowary
{
    public class ClPoint
    {
        public ClPoint(double x, double y)
        {
            X = new ClVariable(x);
            Y = new ClVariable(y);
        }

        public ClPoint(double x, double y, int a)
        {
            X = new ClVariable("x" + a, x);
            Y = new ClVariable("y" + a, y);
        }

        public ClPoint(ClVariable clvX, ClVariable clvY)
        {
            X = clvX;
            Y = clvY;
        }

        public ClVariable X { get; set; }
        public ClVariable Y { get; set; }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }

    public static class CassowarySimplexSolverPointExtensions
    {
        /// <summary>
        /// Add weak stays to the x and y parts of each point. These
        /// have increasing weights so that the solver will try to satisfy
        /// the x and y stays on the same point, rather than the x stay on
        /// one and the y stay on another.
        /// <param name="points">
        /// List of points to add weak stay constraints for.
        /// </param>
        /// </summary>
        public static ClSimplexSolver AddPointStays(this ClSimplexSolver solver, IEnumerable<ClPoint> points)
        {
            double weight = 1.0;
            const double MULTIPLIER = 2.0;

            foreach (ClPoint p in points)
            {
                solver.AddPointStay(p, weight);
                weight *= MULTIPLIER;
            }

            return solver;
        }

        public static ClSimplexSolver AddPointStay(this ClSimplexSolver solver, ClVariable vx, ClVariable vy, double weight)
        {
            solver.AddStay(vx, ClStrength.Weak, weight);
            solver.AddStay(vy, ClStrength.Weak, weight);

            return solver;
        }

        public static ClSimplexSolver AddPointStay(this ClSimplexSolver solver, ClVariable vx, ClVariable vy)
        {
            solver.AddPointStay(vx, vy, 1.0);

            return solver;
        }

        public static ClSimplexSolver AddPointStay(this ClSimplexSolver solver, ClPoint clp, double weight)
        {
            solver.AddStay(clp.X, ClStrength.Weak, weight);
            solver.AddStay(clp.Y, ClStrength.Weak, weight);

            return solver;
        }

        public static ClSimplexSolver AddPointStay(this ClSimplexSolver solver, ClPoint clp)
        {
            solver.AddPointStay(clp, 1.0);

            return solver;
        }
    }
}