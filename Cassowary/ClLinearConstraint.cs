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

namespace Cassowary_moddiff
{
    /// <summary>
    /// Represents linear inequality constraint
    /// </summary>
    public class ClLinearConstraint : ClConstraint
    {
        
        public ClLinearConstraint(ClLinearExpression cle, ClStrength strength, double weight)
            : base(strength, weight)
        {
            ExpressionField = cle.Clone();
        }

        public ClLinearConstraint(ClLinearExpression cle, ClStrength strength)
            : base(strength)
        {
            ExpressionField = cle.Clone();
        }

        public ClLinearConstraint(ClLinearExpression cle)
            : base(ClStrength.Default)
        {
            ExpressionField = cle.Clone();
        }

        public ClLinearConstraint(ClLinearExpression cle, Cl.Operator op) : this(cle, op, ClStrength.Default) { }

        public ClLinearConstraint(ClLinearExpression cle, Cl.Operator op, ClStrength strength, double weight = 1.0) : this(cle, strength, weight)
        /* throws ExClInternalError */
        {
            isInequality = op != Cl.Operator.EqualTo;

            switch (op)
            {
                case Cl.Operator.EqualTo:
                case Cl.Operator.GreaterThanOrEqualTo:
                    break;
                case Cl.Operator.LessThanOrEqualTo:
                    ExpressionField.MultiplyMe(-1.0);
                    break;
            }
        }


        public ClLinearConstraint(ClLinearExpression cle1, Cl.Operator op, ClLinearExpression cle2)
            : this(cle1, op, cle2, ClStrength.Default) { }
            /* throws ExClInternalError */
        
        public ClLinearConstraint(ClLinearExpression cle1, Cl.Operator op, ClLinearExpression cle2, ClStrength strength, double weight = 1.0)
            : this(cle1, strength, weight)
        /* throws ExClInternalError */
        {
            isInequality = op != Cl.Operator.EqualTo;

            ExpressionField.AddExpression(cle2, -1.0);

            switch (op)
            {
                case Cl.Operator.EqualTo:
                case Cl.Operator.GreaterThanOrEqualTo:
                    break;
                case Cl.Operator.LessThanOrEqualTo:
                    ExpressionField.MultiplyMe(-1.0);
                    break;
            }

        }


        public ClLinearConstraint(ClLinearExpression cle1, ClLinearExpression cle2, ClStrength strength, double weight = 1.0)
    : this(cle1, Cl.Operator.EqualTo, cle2, strength, weight) { }
        /* throws ExClInternalError */


        bool isInequality = false;
        public override bool IsInequality
        {
            get
            {
                return isInequality;
            }
        }

        public override ClLinearExpression Expression
        {
            get { return ExpressionField; }
        }

        protected void SetExpression(ClLinearExpression expr)
        {
            ExpressionField = expr;
        }

        protected ClLinearExpression ExpressionField;

        public override string ToString()
        {
            if (IsInequality)
            {
                return CreateDescription("≥", "≤");
            }
            else
            {
                return CreateDescription("=", "=");
            }
        }
    }
}