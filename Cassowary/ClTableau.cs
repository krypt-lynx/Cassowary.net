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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cassowary
{
    public class ClTableau : Cl
    {
        /// <summary>
        /// Constructor is protected, since this only supports an ADT for
        /// the ClSimplexSolver class.
        /// </summary>
        protected ClTableau()
        {
            Columns = new Dictionary<ClAbstractVariable, HashSet<ClAbstractVariable>>();
            Rows = new Dictionary<ClAbstractVariable, ClLinearExpression>();
            InfeasibleRows = new HashSet<ClAbstractVariable>();
            ExternalRows = new HashSet<ClVariable>();
            ExternalParametricVars = new HashSet<ClVariable>();
        }

        /// <summary>
        /// Variable v has been removed from an expression. If the
        /// expression is in a tableau the corresponding basic variable is
        /// subject (or if subject is nil then it's in the objective function).
        /// Update the column cross-indices.
        /// </summary>
        public void NoteRemovedVariable(ClAbstractVariable v, ClAbstractVariable subject)
        {
            if (subject != null)
            {
                Columns[v].Remove(subject);
            }
        }

        /// <summary>
        /// v has been added to the linear expression for subject
        /// update column cross indices.
        /// </summary>
        public void NoteAddedVariable(ClAbstractVariable v, ClAbstractVariable subject)
        {
            if (subject != null)
            {
                InsertColVar(v, subject);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(base.ToString());

            sb.AppendLine("Tableau:");

            foreach (ClAbstractVariable clv in Rows.Keys)
            {
                ClLinearExpression expr = Rows[clv];
                sb.AppendFormat("{0} <==> {1}\n", clv, expr);
            }

            sb.AppendFormat("\nColumns:\n{0}", string.Join("\n", Columns.Select(kvp => $"[{kvp.Key}, {{{string.Join(", ", kvp.Value)}}}]")));
            sb.AppendFormat("\n\nInfeasible rows:\n{{{0}}}", string.Join(", ", InfeasibleRows));
            sb.AppendFormat("\n\nExternal basic variables:\n{{{0}}}", string.Join(", ", ExternalRows));
            sb.AppendFormat("\n\nExternal parametric variables:\n{{{0}}}", string.Join(", ", ExternalParametricVars));

            return sb.ToString();
        }


        /// <summary>
        /// Convenience function to insert a variable into
        /// the set of rows stored at _columns[param_var],
        /// creating a new set if needed. 
        /// </summary>
        private void InsertColVar(ClAbstractVariable paramVar, ClAbstractVariable rowvar)
        {
            HashSet<ClAbstractVariable> rowset;
            if (!Columns.TryGetValue(paramVar, out rowset))
                Columns.Add(paramVar, rowset = new HashSet<ClAbstractVariable>());

            rowset.Add(rowvar);
        }

        /// <summary>
        /// Add v=expr to the tableau, update column cross indices
        /// v becomes a basic variable
        /// expr is now owned by ClTableau class, 
        /// and ClTableau is responsible for deleting it
        /// (also, expr better be allocated on the heap!).
        /// </summary>
        /// <param name="var"></param>
        /// <param name="expr"></param>
        protected void AddRow(ClAbstractVariable var, ClLinearExpression expr)
        {
            // for each variable in expr, add var to the set of rows which
            // have that variable in their expression
            Rows.Add(var, expr);

            // FIXME: check correctness!
            foreach (var clv in expr.Terms.Keys)
            {
                InsertColVar(clv, var);

                if (clv.IsExternal)
                {
                    ExternalParametricVars.Add((ClVariable)clv);
                }
            }

            if (var.IsExternal)
            {
                ExternalRows.Add((ClVariable)var);
            }
        }

        /// <summary>
        /// Remove v from the tableau -- remove the column cross indices for v
        /// and remove v from every expression in rows in which v occurs
        /// </summary>
        protected void RemoveColumn(ClAbstractVariable var)
        {
            // remove the rows with the variables in varset

            HashSet<ClAbstractVariable> rows;
            if (Columns.TryGetValue(var, out rows))
            {
                Columns.Remove(var);

                foreach (var expr in rows.Select(clv => Rows[clv]))
                    expr.Terms.Remove(var);
            }

            if (var.IsExternal)
            {
                ExternalRows.Remove((ClVariable)var);
                ExternalParametricVars.Remove((ClVariable)var);
            }
        }

        /// <summary>
        /// Remove the basic variable v from the tableau row v=expr
        /// Then update column cross indices.
        /// </summary>
        protected ClLinearExpression RemoveRow(ClAbstractVariable var)
            /*throws ExCLInternalError*/
        {
            var expr = Rows[var];
            if (expr == null)
                throw new CassowaryInternalException("linear expression is null");

            // For each variable in this expression, update
            // the column mapping and remove the variable from the list
            // of rows it is known to be in.
            foreach (var varset in expr.Terms.Keys.Select(clv => Columns[clv]).Where(varset => varset != null))
                varset.Remove(var);

            InfeasibleRows.Remove(var);

            if (var.IsExternal)
            {
                ExternalRows.Remove((ClVariable)var);
            }

            Rows.Remove(var);
            return expr;
        }

        /// <summary> 
        /// Replace all occurrences of oldVar with expr, and update column cross indices
        /// oldVar should now be a basic variable.
        /// </summary> 
        protected void SubstituteOut(ClAbstractVariable oldVar, ClLinearExpression expr)
        {
            var varset = Columns[oldVar];

            foreach (var v in varset)
            {
                var row = Rows[v];
                row.SubstituteOut(oldVar, expr, v, this);
                if (v.IsRestricted && row.Constant < 0.0)
                {
                    InfeasibleRows.Add(v);
                }
            }

            if (oldVar.IsExternal)
            {
                ExternalRows.Add((ClVariable)oldVar);
                ExternalParametricVars.Remove((ClVariable)oldVar);
            }

            Columns.Remove(oldVar);
        }

        /// <summary>
        /// _columns is a mapping from variables which occur in expressions to the
        /// set of basic variables whose expressions contain them
        /// i.e., it's a mapping from variables in expressions (a column) to the 
        /// set of rows that contain them.
        /// </summary>
        protected Dictionary<ClAbstractVariable, HashSet<ClAbstractVariable>> Columns { get; }

        /// <summary>
        /// _rows maps basic variables to the expressions for that row in the tableau.
        /// </summary>
        protected Dictionary<ClAbstractVariable, ClLinearExpression> Rows { get; }

        /// <summary>
        /// Return true if and only if the variable subject is in the columns keys 
        /// </summary>
        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool ColumnsHasKey(ClAbstractVariable subject)
        {
            return Columns.ContainsKey(subject);
        }*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ClLinearExpression RowExpression(ClAbstractVariable v)
        {
            // if (Trace) FnEnterPrint(string.Format("rowExpression: {0}", v));
            ClLinearExpression exp;
            Rows.TryGetValue(v, out exp);
            return exp;
        }

        /// <summary>
        /// Collection of basic variables that have infeasible rows
        /// (used when reoptimizing).
        /// </summary>
        protected readonly HashSet<ClAbstractVariable> InfeasibleRows; // Set of ClAbstractVariable-s

        /// <summary>
        /// Set of rows where the basic variable is external
        /// this was added to the Java/C++/C# versions to reduce time in SetExternalVariables().
        /// </summary>
        protected readonly HashSet<ClVariable> ExternalRows;

        /// <summary>
        /// Set of external variables which are parametric
        /// this was added to the Java/C++/C# versions to reduce time in SetExternalVariables().
        /// </summary>
        protected readonly HashSet<ClVariable> ExternalParametricVars;
    }
}