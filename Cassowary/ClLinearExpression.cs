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
using System.Text;

namespace Cassowary
{
    public partial class ClLinearExpression : Cl
    {
        public ClLinearExpression(ClAbstractVariable clv, double value = 1, double constant = 0)
        {
            _constant = constant;
            Terms = new Dictionary<ClAbstractVariable, double>();

            if (clv != null)
                Terms.Add(clv, value);
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
        protected ClLinearExpression(double constant, Dictionary<ClAbstractVariable, double> terms)
        {
            _constant = constant;
            Terms = new Dictionary<ClAbstractVariable, double>();

            // need to unalias the ClDouble-s that we clone (do a deep clone)
            foreach (var clv in terms.Keys)
            {
                Terms.Add(clv, terms[clv]);
            }
        }

        public ClLinearExpression MultiplyMe(double x)
        {
            _constant = _constant * x;

            foreach (var key in Terms.Keys.ToArray())
                Terms[key] = Terms[key] * x;

            return this;
        }

        public ClLinearExpression DivideMe(double x)
        {
            if (Approx(x, 0.0))
            {
                throw new CassowaryNonlinearExpressionException();
            }

            _constant = _constant / x;

            foreach (var key in Terms.Keys.ToArray())
                Terms[key] = Terms[key] / x;
            
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
                return expr.Times(_constant);

            if (!expr.IsConstant)
                throw new CassowaryNonlinearExpressionException();

            return Times(expr._constant);
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

            return Divide(expr._constant);
        }

        public ClLinearExpression DivFrom(ClLinearExpression expr)
            /*throws ExCLNonlinearExpression*/
        {
            if (!IsConstant || Approx(_constant, 0.0))
            {
                throw new CassowaryNonlinearExpressionException();
            }

            return expr.Divide(_constant);
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
                double coeff = expr.Terms[clv];
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
                double coeff = expr.Terms[clv];
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
            double coeff;

            if (Terms.TryGetValue(v, out coeff))
            {
                double newCoefficient = coeff + c;

                if (Approx(newCoefficient, 0.0))
                {
                    Terms.Remove(v);
                }
                else
                {
                    Terms[v] = newCoefficient;
                }
            }
            else
            {
                if (!Approx(c, 0.0))
                {
                    Terms.Add(v, c);
                }
            }

            return this;
        }

        public ClLinearExpression SetVariable(ClAbstractVariable v, double c)
        {
            // Assert(c != 0.0);

            if (Terms.ContainsKey(v))
                Terms[v] = c;
            else
                Terms.Add(v, c);

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
            double coeff;

            if (Terms.TryGetValue(v, out coeff))
            {
                double newCoefficient = coeff + c;

                if (Approx(newCoefficient, 0.0))
                {
                    solver.NoteRemovedVariable(v, subject);
                    Terms.Remove(v);
                }
                else
                {
                    Terms[v] = newCoefficient;
                }
            }
            else
            {
                if (!Approx(c, 0.0))
                {
                    Terms.Add(v, c);
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
            double multiplier = Terms[var];
            Terms.Remove(var);
            IncrementConstant(multiplier * expr.Constant);

            foreach (ClAbstractVariable clv in expr.Terms.Keys)
            {
                double coeff = expr.Terms[clv];
                double dOldCoeff;

                if (Terms.TryGetValue(clv, out dOldCoeff))
                {
                    double newCoeff = dOldCoeff + multiplier * coeff;

                    if (Approx(newCoeff, 0.0))
                    {
                        solver.NoteRemovedVariable(clv, subject);
                        Terms.Remove(clv);
                    }
                    else
                    {
                        Terms[clv] = newCoeff;
                    }
                }
                else
                {
                    // did not have that variable already
                    Terms.Add(clv, multiplier * coeff);
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
            if (Terms.ContainsKey(oldSubject))
                Terms[oldSubject] = NewSubject(newSubject);
            else
                Terms.Add(oldSubject, NewSubject(newSubject));
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
            double coeff = Terms[subject];
            Terms.Remove(subject);

            double reciprocal = 1.0 / coeff;
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
            if (Terms.ContainsKey(var))
                return Terms[var];
            else
                return 0.0;
        }

        public double Constant
        {
            get { return _constant; }
            set { _constant = value; }
        }

        public Dictionary<ClAbstractVariable, double> Terms { get; }

        public void IncrementConstant(double c)
        {
            _constant = _constant + c;
        }

        public bool IsConstant
        {
            get { return Terms.Count == 0; }
        }

       /* public override string ToString()
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
        }*/


        private double _constant;

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (_constant != 0)
            {
                sb.Append(_constant);
            }

            foreach (var kvp in Terms)
            {
                var value = kvp.Value;
                if (value != 0)
                {
                    if (value > 0)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(" + ");
                        }
                    }
                    else
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(" - ");
                        }
                        else
                        {
                            sb.Append("-");
                        }

                    }

                    if (Math.Abs(value) != 1)
                    {
                        sb.Append($"{Math.Abs(value)}×");
                    }
                    sb.Append(kvp.Key);
                }
            }


            return sb.ToString();
        }
    }
}