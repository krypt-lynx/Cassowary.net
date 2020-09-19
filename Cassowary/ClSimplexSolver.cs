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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cassowary_moddiff
{
    /// <summary>Simplex solver. Resolves set of constraints using system of linear equations</summary>
    public class ClSimplexSolver : ClTableau, IEditContext
    {
        public string Name;

        /// <summary>
        /// Constructor initializes the fields, and creaties the objective row.
        /// </summary>
        public ClSimplexSolver()
        {
            Rows.Add(_objective, new ClLinearExpression());
        }

        /// <summary>
        /// Add a constraint to the solver.
        /// <param name="cn">
        /// The constraint to be added.
        /// </param>
        /// </summary>
        public ClSimplexSolver AddConstraint(ClConstraint cn)
        {
            List<ClAbstractVariable> eplusEminus = new List<ClAbstractVariable>(2);
            double prevEConstant = 0;
            ClLinearExpression expr = NewExpression(cn, /* output to: */
                eplusEminus,
                ref prevEConstant);

            bool cAddedOkDirectly = TryAddingDirectly(expr);
            if (!cAddedOkDirectly)
            {
                // could not add directly
                AddWithArtificialVariable(expr);
            }

            _cNeedsSolving = true;

            if (cn.IsEditConstraint)
            {
                int i = _editVarMap.Count;
                ClEditConstraint cnEdit = (ClEditConstraint)cn;
                ClSlackVariable clvEplus = (ClSlackVariable)eplusEminus[0];
                ClSlackVariable clvEminus = (ClSlackVariable)eplusEminus[1];
                _editVarMap.Add(cnEdit.Variable,
                    new ClEditInfo(cnEdit, clvEplus, clvEminus,
                        prevEConstant,
                        i));
            }

            if (AutoSolve)
            {
                Optimize(_objective);
                SetExternalVariables();
            }

            return this;
        }

        /// <summary>
        /// Same as AddConstraint, throws no exceptions.
        /// <returns>
        /// False if the constraint resulted in an unsolvable system, otherwise true.
        /// </returns>
        /// </summary>
        public bool TryAddConstraint(ClConstraint cn)
        {
            try
            {
                AddConstraint(cn);
                return true;
            }
            catch (CassowaryRequiredFailureException)
            {
                return false;
            }
        }

        #region editing
        /// <summary>
        /// Add an edit constraint for a variable with a given strength.
        /// <param name="v">Variable to add an edit constraint to.</param>
        /// <param name="strength">Strength of the edit constraint.</param>
        /// </summary>
        private void AddEditVar(ClVariable v, ClStrength strength)
        {
            try
            {
                AddConstraint(new ClEditConstraint(v, strength));
            }
            catch (CassowaryRequiredFailureException)
            {
                // should not get this
                throw new CassowaryInternalException("Required failure when adding an edit variable");
            }
        }

        /// <summary>
        /// Remove the edit constraint previously added.
        /// <param name="v">Variable to which the edit constraint was added before.</param>
        /// </summary>
        private void RemoveEditVar(ClVariable v)
        {
            ClEditInfo cei = _editVarMap[v];
            ClConstraint cn = cei.Constraint;
            RemoveConstraint(cn);
        }

        /// <summary>
        /// Marks the start of an edit session.
        /// </summary>
        /// <remarks>
        /// BeginEdit should be called before sending Resolve()
        /// messages, after adding the appropriate edit variables.
        /// </remarks>
        public IEditContext BeginEdit(params ClVariable[] editVar)
        {
            foreach (ClVariable variable in editVar)
                AddEditVar(variable, ClStrength.Strong);

            Assert(_editVarMap.Count > 0, "_editVarMap.Count > 0");
            // may later want to do more in here
            InfeasibleRows.Clear();
            ResetStayConstants();
            _stkCedcns.Push(_editVarMap.Count);

            return this;
        }

        /// <summary>
        /// Marks the end of an edit session.
        /// </summary>
        /// <remarks>
        /// EndEdit should be called after editing has finished for now, it
        /// just removes all edit variables.
        /// </remarks>
        ClSimplexSolver IEditContext.EndEdit()
        {
            Assert(_editVarMap.Count > 0, "_editVarMap.Count > 0");
            ((IEditContext)this).Resolve();
            _stkCedcns.Pop();
            int n = _stkCedcns.Peek();
            RemoveEditVarsTo(n);
            // may later want to do more in hore

            return this;
        }

        /// <summary>
        /// Remove the last added edit vars to leave only
        /// a specific number left.
        /// <param name="n">
        /// Number of edit variables to keep.
        /// </param>
        /// </summary>
        private void RemoveEditVarsTo(int n)
        {
            try
            {
                foreach (var kvp in _editVarMap.Where(a => a.Value.Index >= n).ToArray())
                    RemoveEditVar(kvp.Key);
                Assert(_editVarMap.Count == n, "_editVarMap.Count == n");
            }
            catch (CassowaryConstraintNotFoundException)
            {
                // should not get this
                throw new CassowaryInternalException("Constraint not found in RemoveEditVarsTo");
            }
        }

        /// <summary>
        /// Re-solve the current collection of constraints, given the new
        /// values for the edit variables that have already been
        /// suggested (see <see cref="IEditContext.SuggestValue"/> method).
        /// </summary>
        IEditContext IEditContext.Resolve()
        {
            DualOptimize();
            SetExternalVariables();
            InfeasibleRows.Clear();
            ResetStayConstants();

            return this;
        }

        /// <summary>
        /// Suggest a new value for an edit variable. 
        /// </summary>
        /// <remarks>
        /// The variable needs to be added as an edit variable and 
        /// BeginEdit() needs to be called before this is called.
        /// The tableau will not be solved completely until after Resolve()
        /// has been called.
        /// </remarks>
        IEditContext IEditContext.SuggestValue(ClVariable v, double x)
        {
            ClEditInfo cei = _editVarMap[v];
            if (cei == null)
            {
                throw new CassowaryException("SuggestValue for variable " + v + ", but var is not an edit variable\n");
            }
            ClSlackVariable clvEditPlus = cei.ClvEditPlus;
            ClSlackVariable clvEditMinus = cei.ClvEditMinus;
            double delta = x - cei.PrevEditConstant;
            cei.PrevEditConstant = x;
            DeltaEditConstant(delta, clvEditPlus, clvEditMinus);

            return this;
        }
        #endregion

        /// <summary>
        /// Add a stay of the given strength (default to ClStrength#Weak)
        /// of a variable to the tableau..
        /// <param name="v">
        /// Variable to add the stay constraint to.
        /// </param>
        /// </summary>
        public ClSimplexSolver AddStay(ClVariable v, ClStrength strength, double weight)
        /* throws ExClRequiredFailure, ExClInternalError */
        {
            ClStayConstraint cn = new ClStayConstraint(v, strength, weight);

            return AddConstraint(cn);
        }

        /// <remarks>
        /// Default to weight 1.0.
        /// </remarks>
        public ClSimplexSolver AddStay(ClVariable v, ClStrength strength)
        /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddStay(v, strength, 1.0);

            return this;
        }

        /// <remarks>
        /// Default to strength ClStrength#Weak.
        /// </remarks>
        public ClSimplexSolver AddStay(ClVariable v)
        /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddStay(v, ClStrength.Weak, 1.0);

            return this;
        }

        /// <summary>
        /// Remove a constraint from the tableau if present
        /// </summary>
        /// <param name="cn">constraint to remove</param>
        /// <returns>true is constraint was removedm false otherwise</returns>
        public bool TryRemoveConstraint(ClConstraint cn)
        {
            if (ConstraintMap.ContainsKey(cn))
            {
                RemoveConstraint(cn);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove a constraint from the tableau.
        /// Also remove any error variable associated with it.
        /// </summary>
        public ClSimplexSolver RemoveConstraint(ClConstraint cn)
        /* throws ExClRequiredFailure, ExClInternalError */
        {
            _cNeedsSolving = true;

            ResetStayConstants();

            ClLinearExpression zRow = RowExpression(_objective);

            HashSet<ClAbstractVariable> eVars;
            if (_errorVars.TryGetValue(cn, out eVars))
            {
                foreach (ClAbstractVariable clv in eVars)
                {
                    ClLinearExpression expr = RowExpression(clv);
                    if (expr == null)
                    {
                        zRow.AddVariable(clv, -cn.Weight *
                                              cn.Strength.SymbolicWeight.AsDouble(),
                            _objective, this);
                    }
                    else // the error variable was in the basis
                    {
                        zRow.AddExpression(expr, -cn.Weight *
                                                 cn.Strength.SymbolicWeight.AsDouble(),
                            _objective, this);
                    }
                }
            }

            ClAbstractVariable marker;
            if (!ConstraintMap.TryGetValue(cn, out marker))
                throw new CassowaryConstraintNotFoundException();

            ConstraintMap.Remove(cn);
            if (!Rows.ContainsKey(marker))
            {
                // not in the basis, so need to do some more work
                var col = Columns[marker];

                ClAbstractVariable exitVar = null;
                double minRatio = 0.0;
                foreach (ClAbstractVariable v in col)
                {
                    if (v.IsRestricted)
                    {
                        ClLinearExpression expr = RowExpression(v);
                        double coeff = expr.CoefficientFor(marker);

                        if (coeff < 0.0)
                        {
                            double r = -expr.Constant / coeff;
                            if (exitVar == null || r < minRatio)
                            {
                                minRatio = r;
                                exitVar = v;
                            }
                        }
                    }
                }

                if (exitVar == null)
                {
                    foreach (ClAbstractVariable v in col)
                    {
                        if (v.IsRestricted)
                        {
                            ClLinearExpression expr = RowExpression(v);
                            double coeff = expr.CoefficientFor(marker);
                            double r = expr.Constant / coeff;
                            if (exitVar == null || r < minRatio)
                            {
                                minRatio = r;
                                exitVar = v;
                            }
                        }
                    }
                }

                if (exitVar == null)
                {
                    // exitVar is still null
                    if (col.Count == 0)
                    {
                        RemoveColumn(marker);
                    }
                    else
                    {
                        // put first element in exitVar
                        var colEnum = col.GetEnumerator();
                        colEnum.MoveNext();
                        exitVar = colEnum.Current;
                    }
                }

                if (exitVar != null && exitVar != _objective)
                {
                    Pivot(marker, exitVar);
                }
            }

            if (Rows.ContainsKey(marker))
            {
                RemoveRow(marker);
            }

            if (eVars != null)
            {
                foreach (ClAbstractVariable v in eVars.Where(a => a != marker))
                    RemoveColumn(v);
            }

            if (cn.IsStayConstraint)
            {
                if (eVars != null)
                {
                    for (int i = 0; i < _stayPlusErrorVars.Count; i++)
                    {
                        eVars.Remove(_stayPlusErrorVars[i]);
                        eVars.Remove(_stayMinusErrorVars[i]);
                    }
                }
            }
            else if (cn.IsEditConstraint)
            {
                Assert(eVars != null, "eVars != null");
                ClEditConstraint cnEdit = (ClEditConstraint)cn;
                ClVariable clv = cnEdit.Variable;
                ClEditInfo cei = _editVarMap[clv];
                ClSlackVariable clvEditMinus = cei.ClvEditMinus;
                RemoveColumn(clvEditMinus);
                _editVarMap.Remove(clv);
            }

            // FIXME: do the remove at top
            if (eVars != null)
            {
                //_errorVars.Remove(eVars);
                _errorVars.Remove(cn);
            }

            if (AutoSolve)
            {
                Optimize(_objective);
                SetExternalVariables();
            }

            return this;
        }

        /// <summary>
        /// Controls wether optimization and setting of external variables is done
        /// automatically or not.
        /// </summary>
        /// <remarks>
        /// By default it is done automatically and <see cref="Solve"/> never needs
        /// to be explicitly called by client code. If <see cref="AutoSolve"/> is
        /// put to false, then <see cref="Solve"/> needs to be invoked explicitly
        /// before using variables' values. 
        /// (Turning off <see cref="AutoSolve"/> while addings lots and lots
        /// of constraints [ala the AddDel test in ClTests] saved about 20 % in
        /// runtime, from 60sec to 54sec for 900 constraints, with 126 failed adds).
        /// </remarks>
        public bool AutoSolve { get; set; } = true;

        public ClSimplexSolver Solve()
        {
            if (_cNeedsSolving)
            {
                Optimize(_objective);
                SetExternalVariables();
            }

            return this;
        }

        public bool ContainsVariable(ClVariable v)
        /* throws ExClInternalError */
        {
            return Columns.ContainsKey(v) || Rows.ContainsKey(v);
        }

        public ClAbstractVariable GetVariable(string name)
        {
            var c = Columns.Keys.SingleOrDefault(a => a.Name == name);
            if (c != null)
                return c;

            var r = Rows.Keys.SingleOrDefault(a => a.Name == name);
            if (r != null)
                return r;

            return null;
        }

        /// <summary>
        /// All known external variables
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClAbstractVariable> AllVariables()
        {
            //foreach (var v in ExternalRows)
            //foreach (var v in ExternalParametricVars)

            foreach (var v in Columns.Keys)
            {
                if (v.IsExternal)
                    yield return v;
            }
            foreach (var v in Rows.Keys) // todo: exclude duplicates?
            {
                if (v.IsExternal)
                    yield return v;
            }
        }

        public ClSimplexSolver AddVar(ClVariable v)
        /* throws ExClInternalError */
        {
            if (!ContainsVariable(v))
            {
                try
                {
                    AddStay(v);
                }
                catch (CassowaryRequiredFailureException)
                {
                    // cannot have a required failure, since we add w/ weak
                    throw new CassowaryInternalException("Error in AddVar -- required failure is impossible");
                }
            }

            return this;
        }
         
        /// <summary>
        /// Removes all constraints using variable 
        /// </summary>
        /// <param name="v">variable</param>
        public void RemoveVariable(ClVariable v)
        {
            List<ClConstraint> listToRemove = new List<ClConstraint>();
            foreach (var cn in ConstraintMap.Keys)
            {
                if (cn.Expression.Terms.ContainsKey(v))
                {
                    listToRemove.Add(cn);
                }
            }

            foreach (var cn in listToRemove)
            {
                RemoveConstraint(cn);
            }

            RemoveColumn(v);
        }

        public ClConstraint[] AllConstraints()
        {
            return ConstraintMap.Keys.ToArray();
        }

        public IEnumerable<ClConstraint> TestGetConstraints(ClVariable v)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendLine("\n_stayPlusErrorVars: ");
            sb.AppendLine($"{{{string.Join(", ", _stayPlusErrorVars)}}}");
            sb.AppendLine("\n_stayMinusErrorVars: ");
            sb.AppendLine($"{{{string.Join(", ", _stayMinusErrorVars)}}}");

            return sb.ToString();
        }

        public Dictionary<ClConstraint, ClAbstractVariable> ConstraintMap { get; } = new Dictionary<ClConstraint, ClAbstractVariable>();

        //// END PUBLIC INTERFACE ////

        /// <summary>
        /// Add the constraint expr=0 to the inequality tableau using an
        /// artificial variable.
        /// </summary>
        /// <remarks>
        /// To do this, create an artificial variable av and add av=expr
        /// to the inequality tableau, then make av be 0 (raise an exception
        /// if we can't attain av=0).
        /// </remarks>
        private void AddWithArtificialVariable(ClLinearExpression expr)
        /* throws ExClRequiredFailure, ExClInternalError */
        {
            ClSlackVariable av = new ClSlackVariable(Interlocked.Increment(ref _artificialCounter), "a");
            ClObjectiveVariable az = new ClObjectiveVariable("az");
            ClLinearExpression azRow = expr.Clone();

            AddRow(az, azRow);
            AddRow(av, expr);

            Optimize(az);

            ClLinearExpression azTableauRow = RowExpression(az);

            if (!Approx(azTableauRow.Constant, 0.0))
            {
                RemoveRow(az);
                RemoveColumn(av);
                throw new CassowaryRequiredFailureException();
            }

            // see if av is a basic variable
            ClLinearExpression e = RowExpression(av);

            if (e != null)
            {
                // find another variable in this row and pivot,
                // so that av becomes parametric
                if (e.IsConstant)
                {
                    // if there isn't another variable in the row
                    // then the tableau contains the equation av=0 --
                    // just delete av's row
                    RemoveRow(av);
                    RemoveRow(az);
                    return;
                }
                ClAbstractVariable entryVar = e.AnyPivotableVariable();
                Pivot(entryVar, av);
            }
            Assert(!Rows.ContainsKey(av), "RowExpression(av) == null)");
            RemoveColumn(av);
            RemoveRow(az);
        }

        /// <summary>
        /// Try to add expr directly to the tableau without creating an
        /// artificial variable.
        /// </summary>
        /// <remarks>
        /// We are trying to add the constraint expr=0 to the appropriate
        /// tableau.
        /// </remarks>
        /// <returns>
        /// True if successful and false if not.
        /// </returns>
        private bool TryAddingDirectly(ClLinearExpression expr)
        /* throws ExClRequiredFailure */
        {
            ClAbstractVariable subject = ChooseSubject(expr);
            if (subject == null)
            {
                return false;
            }
            expr.NewSubject(subject);
            if (Columns.ContainsKey(subject))
            {
                SubstituteOut(subject, expr);
            }
            AddRow(subject, expr);
            return true; // succesfully added directly
        }

        /// <summary>
        /// Try to choose a subject (a variable to become basic) from
        /// among the current variables in expr.
        /// </summary>
        /// <remarks>
        /// We are trying to add the constraint expr=0 to the tableaux.
        /// If expr constains any unrestricted variables, then we must choose
        /// an unrestricted variable as the subject. Also if the subject is
        /// new to the solver, we won't have to do any substitutions, so we
        /// prefer new variables to ones that are currently noted as parametric.
        /// If expr contains only restricted variables, if there is a restricted
        /// variable with a negative coefficient that is new to the solver we can
        /// make that the subject. Otherwise we can't find a subject, so return nil.
        /// (In this last case we have to add an artificial variable and use that
        /// variable as the subject -- this is done outside this method though.)
        /// </remarks>
        private ClAbstractVariable ChooseSubject(ClLinearExpression expr)
        /* ExClRequiredFailure */
        {
            ClAbstractVariable subject = null; // the current best subject, if any

            bool foundUnrestricted = false;
            bool foundNewRestricted = false;

            var terms = expr.Terms;

            foreach (ClAbstractVariable v in terms.Keys)
            {
                double c = terms[v];

                if (foundUnrestricted)
                {
                    if (!v.IsRestricted)
                    {
                        if (!Columns.ContainsKey(v))
                            return v;
                    }
                }
                else
                {
                    // we haven't found an restricted variable yet
                    if (v.IsRestricted)
                    {
                        if (!foundNewRestricted && !v.IsDummy && c < 0.0)
                        {
                            HashSet<ClAbstractVariable> col;
                            if (!Columns.TryGetValue(v, out col) ||
                                (col.Count == 1 && Columns.ContainsKey(_objective)))
                            {
                                subject = v;
                                foundNewRestricted = true;
                            }
                        }
                    }
                    else
                    {
                        subject = v;
                        foundUnrestricted = true;
                    }
                }
            }

            if (subject != null)
                return subject;

            double coeff = 0.0;

            foreach (ClAbstractVariable v in terms.Keys)
            {
                double c = terms[v];

                if (!v.IsDummy)
                    return null; // nope, no luck
                if (!Columns.ContainsKey(v))
                {
                    subject = v;
                    coeff = c;
                }
            }

            if (!Approx(expr.Constant, 0.0))
            {
                throw new CassowaryRequiredFailureException();
            }
            if (coeff > 0.0)
            {
                expr.MultiplyMe(-1);
            }

            return subject;
        }

        private ClLinearExpression NewExpression(ClConstraint cn,
            ICollection<ClAbstractVariable> eplusEminus,
            ref double prevEConstant)
        {
            ClLinearExpression cnExpr = cn.Expression;
            ClLinearExpression expr = new ClLinearExpression(cnExpr.Constant);
            ClSlackVariable eminus;
            var cnTerms = cnExpr.Terms;
            foreach (var kvp in cnTerms)
            {
                ClLinearExpression e = RowExpression(kvp.Key);
                if (e == null)
                    expr.AddVariable(kvp.Key, kvp.Value);
                else
                    expr.AddExpression(e, kvp.Value);
            }

            if (cn.IsInequality)
            {
                ClSlackVariable slackVar = new ClSlackVariable(Interlocked.Increment(ref _slackCounter), "s");
                expr.SetVariable(slackVar, -1);
                ConstraintMap.Add(cn, slackVar);
                if (!cn.Strength.IsRequired)
                {
                    eminus = new ClSlackVariable(Interlocked.Increment(ref _slackCounter), "em");
                    expr.SetVariable(eminus, 1.0);
                    ClLinearExpression zRow = RowExpression(_objective);
                    ClSymbolicWeight sw = cn.Strength.SymbolicWeight.Times(cn.Weight);
                    zRow.SetVariable(eminus, sw.AsDouble());
                    InsertErrorVar(cn, eminus);
                    NoteAddedVariable(eminus, _objective);
                }
            }
            else
            {
                // cn is an equality
                if (cn.Strength.IsRequired)
                {
                    ClDummyVariable dummyVar = new ClDummyVariable(Interlocked.Increment(ref _dummyCounter), "d");
                    expr.SetVariable(dummyVar, 1.0);
                    ConstraintMap.Add(cn, dummyVar);
                }
                else
                {
                    var slackCounter = Interlocked.Increment(ref _slackCounter);
                    ClSlackVariable eplus = new ClSlackVariable(slackCounter, "ep");
                    eminus = new ClSlackVariable(slackCounter, "em");

                    expr.SetVariable(eplus, -1.0);
                    expr.SetVariable(eminus, 1.0);
                    ConstraintMap.Add(cn, eplus);
                    ClLinearExpression zRow = RowExpression(_objective);
                    ClSymbolicWeight sw = cn.Strength.SymbolicWeight.Times(cn.Weight);
                    double swCoeff = sw.AsDouble();
                    zRow.SetVariable(eplus, swCoeff);
                    NoteAddedVariable(eplus, _objective);
                    zRow.SetVariable(eminus, swCoeff);
                    NoteAddedVariable(eminus, _objective);
                    InsertErrorVar(cn, eminus);
                    InsertErrorVar(cn, eplus);
                    if (cn.IsStayConstraint)
                    {
                        _stayPlusErrorVars.Add(eplus);
                        _stayMinusErrorVars.Add(eminus);
                    }
                    else if (cn.IsEditConstraint)
                    {
                        eplusEminus.Add(eplus);
                        eplusEminus.Add(eminus);
                        prevEConstant = cnExpr.Constant;
                    }
                }
            }

            if (expr.Constant < 0)
                expr.MultiplyMe(-1);

            return expr;
        }

        /// <summary>
        /// Minimize the value of the objective.
        /// </summary>
        /// <remarks>
        /// The tableau should already be feasible.
        /// </remarks>
        private void Optimize(ClObjectiveVariable zVar)
        /* throws ExClInternalError */
        {
            ClLinearExpression zRow = RowExpression(zVar);
            if (zRow == null)
                throw new CassowaryInternalException("Assertion failed: zRow != null");

            ClAbstractVariable entryVar = null;
            ClAbstractVariable exitVar = null;
            while (true)
            {
                double objectiveCoeff = 0;
                foreach (var kvp in zRow.Terms)
                {
                    if (kvp.Key.IsPivotable && kvp.Value < objectiveCoeff)
                    {
                        objectiveCoeff = kvp.Value;
                        entryVar = kvp.Key;
                    }
                }

                if (objectiveCoeff >= -EPSILON || entryVar == null)
                    return;

                double minRatio = Double.MaxValue;
                foreach (ClAbstractVariable v in Columns[entryVar])
                {
                    if (v.IsPivotable)
                    {
                        ClLinearExpression expr = RowExpression(v);
                        double coeff = expr.CoefficientFor(entryVar);
                        if (coeff < 0.0)
                        {
                            double r = -expr.Constant / coeff;
                            if (r < minRatio)
                            {
                                minRatio = r;
                                exitVar = v;
                            }
                        }
                    }
                }
                // ReSharper disable CompareOfFloatsByEqualityOperator
                if (minRatio == Double.MaxValue)
                // ReSharper restore CompareOfFloatsByEqualityOperator
                {
                    throw new CassowaryInternalException("Objective function is unbounded in Optimize");
                }
                Pivot(entryVar, exitVar);
            }
        }

        /// <summary>
        /// Fix the constants in the equations representing the edit constraints.
        /// </summary>
        /// <remarks>
        /// Each of the non-required edits will be represented by an equation
        /// of the form:
        ///   v = c + eplus - eminus
        /// where v is the variable with the edit, c is the previous edit value,
        /// and eplus and eminus are slack variables that hold the error in 
        /// satisfying the edit constraint. We are about to change something,
        /// and we want to fix the constants in the equations representing
        /// the edit constraints. If one of eplus and eminus is basic, the other
        /// must occur only in the expression for that basic error variable. 
        /// (They can't both be basic.) Fix the constant in this expression.
        /// Otherwise they are both non-basic. Find all of the expressions
        /// in which they occur, and fix the constants in those. See the
        /// UIST paper for details.
        /// (This comment was for ResetEditConstants(), but that is now
        /// gone since it was part of the screwey vector-based interface
        /// to resolveing. --02/16/99 gjb)
        /// </remarks>
        protected void DeltaEditConstant(double delta,
            ClAbstractVariable plusErrorVar,
            ClAbstractVariable minusErrorVar)
        {
            ClLinearExpression exprPlus = RowExpression(plusErrorVar);
            if (exprPlus != null)
            {
                exprPlus.IncrementConstant(delta);

                if (exprPlus.Constant < 0.0)
                {
                    InfeasibleRows.Add(plusErrorVar);
                }
                return;
            }

            ClLinearExpression exprMinus = RowExpression(minusErrorVar);
            if (exprMinus != null)
            {
                exprMinus.IncrementConstant(-delta);
                if (exprMinus.Constant < 0.0)
                {
                    InfeasibleRows.Add(minusErrorVar);
                }
                return;
            }

            var columnVars = Columns[minusErrorVar];

            foreach (ClAbstractVariable basicVar in columnVars)
            {
                ClLinearExpression expr = RowExpression(basicVar);
                //Assert(expr != null, "expr != null");
                double c = expr.CoefficientFor(minusErrorVar);
                expr.IncrementConstant(c * delta);
                if (basicVar.IsRestricted && expr.Constant < 0.0)
                {
                    InfeasibleRows.Add(basicVar);
                }
            }
        }

        /// <summary>
        /// Re-optimize using the dual simplex algorithm.
        /// </summary>
        /// <remarks>
        /// We have set new values for the constants in the edit constraints.
        /// </remarks>
        protected void DualOptimize()
        /* throws ExClInternalError */
        {
            ClLinearExpression zRow = RowExpression(_objective);
            while (InfeasibleRows.Count > 0)
            {
                ClAbstractVariable exitVar = InfeasibleRows.First();

                InfeasibleRows.Remove(exitVar);
                ClAbstractVariable entryVar = null;
                ClLinearExpression expr = RowExpression(exitVar);
                if (expr != null)
                {
                    if (expr.Constant < 0.0)
                    {
                        double ratio = Double.MaxValue;
                        var terms = expr.Terms;
                        foreach (ClAbstractVariable v in terms.Keys)
                        {
                            double c = terms[v];
                            if (c > 0.0 && v.IsPivotable)
                            {
                                double zc = zRow.CoefficientFor(v);
                                double r = zc / c;
                                if (r < ratio)
                                {
                                    entryVar = v;
                                    ratio = r;
                                }
                            }
                        }
                        // ReSharper disable CompareOfFloatsByEqualityOperator
                        if (ratio == Double.MaxValue)
                        // ReSharper restore CompareOfFloatsByEqualityOperator
                        {
                            throw new CassowaryInternalException("ratio == nil (Double.MaxValue) in DualOptimize");
                        }
                        Pivot(entryVar, exitVar);
                    }
                }
            }
        }

        /// <summary>
        /// Do a pivot. Move entryVar into the basis and move exitVar 
        /// out of the basis.
        /// </summary>
        /// <remarks>
        /// We could for example make entryVar a basic variable and
        /// make exitVar a parametric variable.
        /// </remarks>
        protected void Pivot(ClAbstractVariable entryVar, ClAbstractVariable exitVar)
        /* throws ExClInternalError */
        {
            // the entryVar might be non-pivotable if we're doing a 
            // RemoveConstraint -- otherwise it should be a pivotable
            // variable -- enforced at call sites, hopefully

            ClLinearExpression pexpr = RemoveRow(exitVar);

            pexpr.ChangeSubject(exitVar, entryVar);
            SubstituteOut(entryVar, pexpr);
            AddRow(entryVar, pexpr);
        }

        /// <summary>
        /// Fix the constants in the equations representing the stays.
        /// </summary>
        /// <remarks>
        /// Each of the non-required stays will be represented by an equation
        /// of the form
        ///   v = c + eplus - eminus
        /// where v is the variable with the stay, c is the previous value
        /// of v, and eplus and eminus are slack variables that hold the error
        /// in satisfying the stay constraint. We are about to change something,
        /// and we want to fix the constants in the equations representing the
        /// stays. If both eplus and eminus are nonbasic they have value 0
        /// in the current solution, meaning the previous stay was exactly
        /// satisfied. In this case nothing needs to be changed. Otherwise one
        /// of them is basic, and the other must occur only in the expression
        /// for that basic error variable. Reset the constant of this
        /// expression to 0.
        /// </remarks>
        protected void ResetStayConstants()
        {
            for (int i = 0; i < _stayPlusErrorVars.Count; i++)
            {
                ClLinearExpression expr =
                    RowExpression(_stayPlusErrorVars[i]);
                if (expr == null)
                    expr = RowExpression(_stayMinusErrorVars[i]);
                if (expr != null)
                    expr.Constant = 0.0;
            }
        }

        /// <summary>
        /// Set the external variables known to this solver to their appropriate values.
        /// </summary>
        /// <remarks>
        /// Set each external basic variable to its value, and set each external parametric
        /// variable to 0. (It isn't clear that we will ever have external parametric
        /// variables -- every external variable should either have a stay on it, or have an
        /// equation that defines it in terms of other external variables that do have stays.
        /// For the moment I'll put this in though.) Variables that are internal to the solver
        /// don't actually store values -- their values are just implicit in the tableau -- so
        /// we don't need to set them.
        /// </remarks>
        protected void SetExternalVariables()
        {
            foreach (var v in ExternalParametricVars)
            {
                if (Rows.ContainsKey(v))
                    Console.Error.WriteLine(string.Format("Error: variable {0} in _externalParametricVars is basic", v));
                else
                    v.Value = 0.0;
            }

            foreach (var v in ExternalRows)
            {
                ClLinearExpression expr = RowExpression(v);
                v.Value = expr.Constant;
            }

            _cNeedsSolving = false;
        }

        /// <summary>
        /// Protected convenience function to insert an error variable
        /// into the _errorVars set, creating the mapping with Add as necessary.
        /// </summary>
        protected void InsertErrorVar(ClConstraint cn, ClAbstractVariable var)
        {
            HashSet<ClAbstractVariable> cnset;
            if (!_errorVars.TryGetValue(cn, out cnset))
                _errorVars.Add(cn, cnset = new HashSet<ClAbstractVariable>());
            cnset.Add(var);
        }

        //// BEGIN PRIVATE INSTANCE FIELDS ////

        /// <summary>
        /// The array of negative error vars for the stay constraints
        /// (need both positive and negative since they have only non-negative
        /// values).
        /// </summary>
        private readonly List<ClSlackVariable> _stayMinusErrorVars = new List<ClSlackVariable>();

        /// <summary>
        /// The array of positive error vars for the stay constraints
        /// (need both positive and negative since they have only non-negative
        /// values).
        /// </summary>
        private readonly List<ClSlackVariable> _stayPlusErrorVars = new List<ClSlackVariable>();

        private static int nextObjective = 0;
        private static int getObjectiveId()
        {
            return Interlocked.Increment(ref nextObjective);
        }
        /// <summary>
        /// Give error variables for a non-required constraints,
        /// maps to ClSlackVariable-s.
        /// </summary>
        /// <remarks>
        /// Map ClConstraint to Set (of ClVariable).
        /// </remarks>
        private readonly Dictionary<ClConstraint, HashSet<ClAbstractVariable>> _errorVars = new Dictionary<ClConstraint, HashSet<ClAbstractVariable>>();
        private readonly ClObjectiveVariable _objective = new ClObjectiveVariable($"Objective_{getObjectiveId()}");

        /// <summary>
        /// Map edit variables to ClEditInfo-s.
        /// </summary>
        /// <remarks>
        /// ClEditInfo instances contain all the information for an
        /// edit constraints (the edit plus/minus vars, the index [for old-style
        /// resolve(ArrayList...)] interface), and the previous value.
        /// (ClEditInfo replaces the parallel vectors from the Smalltalk impl.)
        /// </remarks>
        private readonly Dictionary<ClVariable, ClEditInfo> _editVarMap = new Dictionary<ClVariable, ClEditInfo>();

        private static long _slackCounter = 0;
        private static long _artificialCounter = 0;
        private static long _dummyCounter = 0;

        private const double EPSILON = 1e-8;
        private bool _cNeedsSolving = false;

        private readonly Stack<int> _stkCedcns = new Stack<int>(new[] { 0 });

        /// <summary>
        /// Is solvers does not contains same variables we can marge them
        /// Second solver must not be used after this operation.
        /// </summary>
        /// <param name="other"></param>
        public void MergeWith(ClSimplexSolver other)
        {
            if (this == other)
            {
                throw new InvalidOperationException("trying to merge solver with itself");
            }

            if (_stkCedcns.Count > 1)
            {
                throw new InvalidOperationException("left solver is in editing state");
            }

            if (other._stkCedcns.Count > 1)
            {
                throw new InvalidOperationException("right solver is in editing state");
            }

            var inf = ExternalRows.Intersect(other.ExternalRows).ToArray();
            if (inf.Length > 0)
            {
                throw new InvalidOperationException($"both solvers ([{this.Name ?? "<null>"}], [{other.Name ?? "<null>"}]) contains variables: {string.Join("; ", inf.Select(x => x.ToString()))}");
            }

            _stayMinusErrorVars.AddRange(other._stayMinusErrorVars);
            _stayPlusErrorVars.AddRange(other._stayPlusErrorVars);

            foreach (var kvp in other._errorVars)
            {
                _errorVars[kvp.Key] = kvp.Value;
            }

            foreach (var kvp in other._editVarMap)
            {
                _editVarMap[kvp.Key] = kvp.Value;
            }

            InfeasibleRows.UnionWith(other.InfeasibleRows);
            ExternalRows.UnionWith(other.ExternalRows);
            ExternalParametricVars.UnionWith(other.ExternalParametricVars);


            foreach (var kvp in other.ConstraintMap)
            {
                ConstraintMap[kvp.Key] = kvp.Value;
            }

            foreach (var kvp in other.Columns)
            {
                var column = kvp.Value;
                if (column.Contains(other._objective))
                {
                    column.Remove(other._objective);
                    column.Add(_objective);
                }
                Columns[kvp.Key] = column;
            }

            foreach (var kvp in other.Rows)
            {
                var row = kvp.Value;
                foreach (var term in row.Terms.Keys.ToArray())
                {
                    if (term == other._objective)
                    {
                        var value = row.Terms[term];
                        row.Terms.Remove(term);
                        row.Terms[_objective] = value;

                    }
                }
                Rows[kvp.Key] = kvp.Value;
            }


            var objectiveRow = RowExpression(_objective);
            var othersObjectiveRow = RowExpression(other._objective);



            foreach (var kvp in othersObjectiveRow.Terms)
            {
                objectiveRow.Terms[kvp.Key] = kvp.Value;
            }

            Rows.Remove(other._objective);

                      
            _cNeedsSolving = _cNeedsSolving || other._cNeedsSolving;
            if (AutoSolve)
            {
                Solve();
            }
        }
    }
}