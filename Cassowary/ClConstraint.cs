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

using System.Globalization;

namespace Cassowary
{

    /// <summary>
    /// linear constraint base class
    /// </summary>
    public abstract partial class ClConstraint
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
        public double Weight { get; } = 1;

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
    }
}