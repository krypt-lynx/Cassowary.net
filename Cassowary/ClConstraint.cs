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
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cassowary
{

    /// <summary>
    /// linear constraint base class
    /// </summary>
    public abstract class ClConstraint
    {
        /// <summary>
        /// Creates constraint with strength
        /// </summary>
        /// <param name="strength">constraint strength</param>
        /// <param name="weight">constraint weight</param>
        protected ClConstraint(ClStrength strength, double weight = 1.0)
        {
            Strength = strength;
            Weight = weight;
        }

        /// <summary>
        /// expression of the constraint
        /// </summary>
        public abstract ClLinearExpression Expression { get; }

        /// <summary>
        /// constraint used for variable edit 
        /// </summary>
        public virtual bool IsEditConstraint
        {
            get { return false; }
        }

        /// <summary>
        /// constraint is inequality
        /// </summary>
        public virtual bool IsInequality
        {
            get { return false; }
        }

        /// <summary>
        ///  constraint represents value stay
        /// </summary>
        public virtual bool IsStayConstraint
        {
            get { return false; }
        }
        /// <summary>
        /// Strength of the constraint
        /// </summary>
        public ClStrength Strength { get; private set; } = ClStrength.Default;

        /// <summary>
        /// temporary method to set streight after constraint creation (ised in AddConstraint extansions). TODO: remove
        /// </summary>
        /// <param name="strength"></param>
        public void SetStrength(ClStrength strength)
        {
            Strength = strength; // TODO: validate constraint is not added or ensure it is safe to modify added constraint 
        }

        /// <summary>
        /// Constraint weight
        /// </summary>
        public double Weight { get; private set; } = 1;

        /// <summary>
        /// temporary method to set weight after constraint creation (ised in AddConstraint extansions). TODO: remove
        /// </summary>
        /// <param name="weight"></param>
        public void SetWeight(double weight)
        {
            Weight = weight; // TODO: validate constraint is not added or ensure it is safe to modify added constraint 
        }
        /*
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            // two curly brackets escape the format, so use three to surround
            // a format expression in brackets!
            //
            // example output: weak:[0,0,1] {1} (23 + -1*[update.height:23]
            return string.Format(CultureInfo.InvariantCulture, "{0} {{{1}}} ({2}", Strength, Weight, Expression);
        }
        */

        /// <summary>
        /// private ToString helper class
        /// </summary>
        class Term
        {
            public ClAbstractVariable var;
            public double mult;

            public Term(ClAbstractVariable var, double mult)
            {
                this.var = var;
                this.mult = mult;
            }

            public override string ToString()
            {
                if (mult == 1)
                {
                    return var.ToString();
                }
                else
                {
                    return $"{mult}×{var}";
                }
            }
        }

        public override string ToString()
        {
            return CreateDescription("+?", "-?");
        }

        protected string CreateDescription(string OperatorString, string OperatorStringInversed)
        {
            // copy expression data in usable form
            var terms = Expression.Terms.Select(x => new Term(x.Key, x.Value)).ToArray();
            var constant = Expression.Constant;

            bool operatorNageted = false;

            // make constant always positive
            if (constant < 0)
            {
                constant = -constant;
                operatorNageted = !operatorNageted;
                for (int i = 0; i < terms.Length; i++)
                {
                    terms[i].mult = -terms[i].mult;
                }
            }

            // find positive and negatinve terms
            var posTerms = new List<Term>();
            var negTerms = new List<Term>();

            foreach (var term in terms)
            {
                if (term.mult != 0)
                {
                    if (term.mult > 0)
                    {
                        posTerms.Add(term);
                    }
                    else
                    {
                        term.mult = -term.mult;
                        negTerms.Add(term);
                    }
                }
            }

            // inverse sides if there is no positive terms except, maybe constant
            bool inverse = posTerms.Count == 0;

            if (inverse)
            {
                var buff = posTerms;
                posTerms = negTerms;
                negTerms = buff;
                operatorNageted = !operatorNageted;
            }

            // build description
            var sb = new StringBuilder();

            for (int i = 0; i < posTerms.Count; i++)
            {
                if (i != 0)
                {
                    sb.Append(" + ");
                }
                sb.Append(posTerms[i]);
            }

            if (!inverse && constant != 0)
            {
                if (posTerms.Count > 0)
                {
                    sb.Append(" + ");
                }
                sb.Append(constant);
            }

            sb.Append($" {(!operatorNageted ? OperatorString : OperatorStringInversed)} ");

            for (int i = 0; i < negTerms.Count; i++)
            {
                if (i != 0)
                {
                    sb.Append(" + ");
                }
                sb.Append(negTerms[i]);
            }

            if (inverse)
            {
                if (negTerms.Count > 0)
                {
                    sb.Append(" + ");
                }
                sb.Append(constant);
            }
            else
            {
                if (negTerms.Count == 0)
                {
                    sb.Append(0);
                }
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}:{1} {{{2}}}", Strength, Weight, sb);
        }
    }
}