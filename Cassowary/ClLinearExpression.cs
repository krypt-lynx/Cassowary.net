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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cassowary
{
    public partial class ClLinearExpression : Cl
    {
        public ClLinearExpression(ClAbstractVariable clv, double value = 1, double constant = 0)
        {
            _constant = new ClDouble(constant);
            Terms = new Dictionary<ClAbstractVariable, ClDouble>();

            if (clv != null)
                Terms.Add(clv, new ClDouble(value));
        }

        public ClLinearExpression(double num)
            : this(null, 0, num)
        {
        }

        public ClLinearExpression()
            : this(0)
        {
        }

        /// <summary>
        /// For use by the clone method.
        /// </summary>
        protected ClLinearExpression(ClDouble constant, Dictionary<ClAbstractVariable, ClDouble> terms)
        {
            _constant = constant.Clone();
            Terms = new Dictionary<ClAbstractVariable, ClDouble>();

            // need to unalias the ClDouble-s that we clone (do a deep clone)
            foreach (var clv in terms.Keys)
            {
                Terms.Add(clv, (terms[clv]).Clone());
            }
        }

        public ClLinearExpression MultiplyMe(double x)
        {
            _constant.Value = _constant.Value * x;

            foreach (var cld in Terms.Keys.Select(a => Terms[a]))
                cld.Value = cld.Value * x;

            return this;
        }

        public ClLinearExpression DivideMe(double x)
        {
            if (Approx(x, 0.0))
            {
                throw new CassowaryNonlinearExpressionException();
            }

            _constant.Value = _constant.Value / x;

            foreach (var cld in Terms.Keys.Select(a => Terms[a]))
                cld.Value = cld.Value / x;

            return this;
        }

        public virtual ClLinearExpression Clone()
        {
            return new ClLinearExpression(_constant, Terms);
        }

        public ClLinearExpression Times(double x)
        {
            return (Clone()).MultiplyMe(x);
        }

        public ClLinearExpression Times(ClLinearExpression expr)
            /*throws ExCLNonlinearExpression*/
        {
            if (IsConstant)
                return expr.Times(_constant.Value);

            if (!expr.IsConstant)
                throw new CassowaryNonlinearExpressionException();

            return Times(expr._constant.Value);
        }

        public ClLinearExpression Plus(ClLinearExpression expr)
        {
            return (Clone()).AddExpression(expr);
        }

        public ClLinearExpression Plus(ClVariable var)
            /*throws ExCLNonlinearExpression*/
        {
            return (Clone()).AddVariable(var);
        }

        public ClLinearExpression Minus(ClLinearExpression expr)
        {
            return (Clone()).AddExpression(expr, -1.0);
        }

        public ClLinearExpression Minus(ClVariable var)
            /*throws ExCLNonlinearExpression*/
        {
            return (Clone()).AddVariable(var, -1.0);
        }

        public ClLinearExpression Divide(double x)
            /*throws ExCLNonlinearExpression*/
        {
            return (Clone()).DivideMe(x);
        }

        public ClLinearExpression Divide(ClLinearExpression expr)
            /*throws ExCLNonlinearExpression*/
        {
            if (!expr.IsConstant)
            {
                throw new CassowaryNonlinearExpressionException();
            }

            return Divide(expr._constant.Value);
        }

        public ClLinearExpression DivFrom(ClLinearExpression expr)
            /*throws ExCLNonlinearExpression*/
        {
            if (!IsConstant || Approx(_constant.Value, 0.0))
            {
                throw new CassowaryNonlinearExpressionException();
            }

            return expr.Divide(_constant.Value);
        }

        public ClLinearExpression SubtractFrom(ClLinearExpression expr)
        {
            return expr.Minus(this);
        }

        /// <summary>
        /// Add n*expr to this expression from another expression expr.
        /// Notify the solver if a variable is added or deleted from this
        /// expression.
        /// </summary>
        public ClLinearExpression AddExpression(ClLinearExpression expr, double n, ClAbstractVariable subject, ClTableau solver)
        {
            IncrementConstant(n * expr.Constant);

            foreach (ClAbstractVariable clv in expr.Terms.Keys)
            {
                double coeff = (expr.Terms[clv]).Value;
                AddVariable(clv, coeff * n, subject, solver);
            }

            return this;
        }

        /// <summary>
        /// Add n*expr to this expression from another expression expr.
        /// </summary>
        public ClLinearExpression AddExpression(ClLinearExpression expr, double n = 1.0)
        {
            IncrementConstant(n * expr.Constant);

            foreach (ClAbstractVariable clv in expr.Terms.Keys)
            {
                double coeff = (expr.Terms[clv]).Value;
                AddVariable(clv, coeff * n);
            }

            return this;
        }

        /// <summary>
        /// Add a term c*v to this expression.  If the expression already
        /// contains a term involving v, add c to the existing coefficient.
        /// If the new coefficient is approximately 0, delete v.
        /// </summary>
        public ClLinearExpression AddVariable(ClAbstractVariable v, double c = 1.0)
        {
            ClDouble coeff;

            if (Terms.TryGetValue(v, out coeff))
            {
                double newCoefficient = coeff.Value + c;

                if (Approx(newCoefficient, 0.0))
                {
                    Terms.Remove(v);
                }
                else
                {
                    coeff.Value = newCoefficient;
                }
            }
            else
            {
                if (!Approx(c, 0.0))
                {
                    Terms.Add(v, new ClDouble(c));
                }
            }

            return this;
        }

        public ClLinearExpression SetVariable(ClAbstractVariable v, double c)
        {
            // Assert(c != 0.0);
            ClDouble coeff;

            if (Terms.TryGetValue(v, out coeff))
                coeff.Value = c;
            else
                Terms.Add(v, new ClDouble(c));

            return this;
        }

        /// <summary>
        /// Add a term c*v to this expression.  If the expression already
        /// contains a term involving v, add c to the existing coefficient.
        /// If the new coefficient is approximately 0, delete v.  Notify the
        /// solver if v appears or disappears from this expression.
        /// </summary>
        public ClLinearExpression AddVariable(ClAbstractVariable v, double c, ClAbstractVariable subject, ClTableau solver)
        {
            ClDouble coeff;

            if (Terms.TryGetValue(v, out coeff) && coeff != null)
            {
                double newCoefficient = coeff.Value + c;

                if (Approx(newCoefficient, 0.0))
                {
                    solver.NoteRemovedVariable(v, subject);
                    Terms.Remove(v);
                }
                else
                {
                    coeff.Value = newCoefficient;
                }
            }
            else
            {
                if (!Approx(c, 0.0))
                {
                    Terms.Add(v, new ClDouble(c));
                    solver.NoteAddedVariable(v, subject);
                }
            }

            return this;
        }

        /// <summary>
        /// Return a pivotable variable in this expression.  (It is an error
        /// if this expression is constant -- signal ExCLInternalError in
        /// that case).  Return null if no pivotable variables
        /// </summary>
        public ClAbstractVariable AnyPivotableVariable()
            /*throws ExCLInternalError*/
        {
            if (IsConstant)
            {
                throw new CassowaryInternalException("anyPivotableVariable called on a constant");
            }

            return Terms.Keys.FirstOrDefault(a => a.IsPivotable);

            // No pivotable variables, so just return null, and let the caller
            // error if needed
        }

        /// <summary>
        /// Replace var with a symbolic expression expr that is equal to it.
        /// If a variable has been added to this expression that wasn't there
        /// before, or if a variable has been dropped from this expression
        /// because it now has a coefficient of 0, inform the solver.
        /// PRECONDITIONS:
        ///   var occurs with a non-zero coefficient in this expression.
        /// </summary>
        public void SubstituteOut(ClAbstractVariable var, ClLinearExpression expr, ClAbstractVariable subject, ClTableau solver)
        {
            double multiplier = (Terms[var]).Value;
            Terms.Remove(var);
            IncrementConstant(multiplier * expr.Constant);

            foreach (ClAbstractVariable clv in expr.Terms.Keys)
            {
                double coeff = (expr.Terms[clv]).Value;
                ClDouble dOldCoeff;

                if (Terms.TryGetValue(clv, out dOldCoeff))
                {
                    double oldCoeff = dOldCoeff.Value;
                    double newCoeff = oldCoeff + multiplier * coeff;

                    if (Approx(newCoeff, 0.0))
                    {
                        solver.NoteRemovedVariable(clv, subject);
                        Terms.Remove(clv);
                    }
                    else
                    {
                        dOldCoeff.Value = newCoeff;
                    }
                }
                else
                {
                    // did not have that variable already
                    Terms.Add(clv, new ClDouble(multiplier * coeff));
                    solver.NoteAddedVariable(clv, subject);
                }
            }
        }

        /// <summary>
        /// This linear expression currently represents the equation
        /// oldSubject=self.  Destructively modify it so that it represents
        /// the equation newSubject=self.
        ///
        /// Precondition: newSubject currently has a nonzero coefficient in
        /// this expression.
        ///
        /// NOTES
        ///   Suppose this expression is c + a*newSubject + a1*v1 + ... + an*vn.
        ///
        ///   Then the current equation is 
        ///       oldSubject = c + a*newSubject + a1*v1 + ... + an*vn.
        ///   The new equation will be
        ///        newSubject = -c/a + oldSubject/a - (a1/a)*v1 - ... - (an/a)*vn.
        ///   Note that the term involving newSubject has been dropped.
        /// </summary>
        public void ChangeSubject(ClAbstractVariable oldSubject, ClAbstractVariable newSubject)
        {
            ClDouble cld;

            if (Terms.TryGetValue(oldSubject, out cld))
                cld.Value = NewSubject(newSubject);
            else
                Terms.Add(oldSubject, new ClDouble(NewSubject(newSubject)));
        }

        /// <summary>
        /// This linear expression currently represents the equation self=0.  Destructively modify it so 
        /// that subject=self represents an equivalent equation.  
        ///
        /// Precondition: subject must be one of the variables in this expression.
        /// NOTES
        ///   Suppose this expression is
        ///     c + a*subject + a1*v1 + ... + an*vn
        ///   representing 
        ///     c + a*subject + a1*v1 + ... + an*vn = 0
        /// The modified expression will be
        ///    subject = -c/a - (a1/a)*v1 - ... - (an/a)*vn
        ///   representing
        ///    subject = -c/a - (a1/a)*v1 - ... - (an/a)*vn
        ///
        /// Note that the term involving subject has been dropped.
        /// Returns the reciprocal, so changeSubject can use it, too
        /// </summary>
        public double NewSubject(ClAbstractVariable subject)
        {
            ClDouble coeff = Terms[subject];
            Terms.Remove(subject);

            double reciprocal = 1.0 / coeff.Value;
            MultiplyMe(-reciprocal);

            return reciprocal;
        }

        /// <summary>
        /// Return the coefficient corresponding to variable var, i.e.,
        /// the 'ci' corresponding to the 'vi' that var is:
        ///      v1*c1 + v2*c2 + .. + vn*cn + c
        /// </summary>
        public double CoefficientFor(ClAbstractVariable var)
        {
            
            ClDouble coeff;

            if (Terms.TryGetValue(var, out coeff) && coeff != null)
                return coeff.Value;
            else
                return 0.0;
        }

        public double Constant
        {
            get { return _constant.Value; }
            set { _constant.Value = value; }
        }

        public Dictionary<ClAbstractVariable, ClDouble> Terms { get; }

        public void IncrementConstant(double c)
        {
            _constant.Value = _constant.Value + c;
        }

        public bool IsConstant
        {
            get { return Terms.Count == 0; }
        }

        public override string ToString()
        {
            String s = "";

            IDictionaryEnumerator e = Terms.GetEnumerator();

            if (!Approx(_constant.Value, 0.0) || Terms.Count == 0)
            {
                s += _constant.ToString();
            }
            else
            {
                if (Terms.Count == 0)
                {
                    return s;
                }
                e.MoveNext(); // go to first element
                ClAbstractVariable clv = (ClAbstractVariable) e.Key;
                ClDouble coeff = Terms[clv];
                s += string.Format("{0}*{1}", coeff, clv);
            }
            while (e.MoveNext())
            {
                ClAbstractVariable clv = (ClAbstractVariable) e.Key;
                ClDouble coeff = Terms[clv];
                s += string.Format(" + {0}*{1}", coeff, clv);
            }

            return s;
        }

        public static bool FEquals(ClLinearExpression e1, ClLinearExpression e2)
        {
            return e1 == e2;
        }

        private readonly ClDouble _constant;
    }
}