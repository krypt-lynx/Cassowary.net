/*
	Cassowary.net: an incremental constraint solver for .NET
	(http://lumumba.uhasselt.be/jo/projects/cassowary.net/)
	
	Copyright (C) 2005-2006	Jo Vermeulen (jo.vermeulen@uhasselt.be)
		
	This program is free software; you can redistribute it and/or
	modify it under the terms of the GNU Lesser General Public License
	as published by the Free Software Foundation; either version 2.1
	of	the License, or (at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
	GNU Lesser General Public License for more details.

	You should have received a copy of the GNU Lesser General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA	 02111-1307, USA.
*/

using System;

namespace Cassowary
{
    /// <summary>
    /// The enumerations from ClLinearConstraint,
    /// and `global' functions that we want easy to access
    /// </summary>
    public class Cl
    {
        protected static void Assert(bool f, string description)
        {
            if (!f)
            {
                throw new CassowaryInternalException(string.Format("Assertion failed: {0}", description));
            }
        }

        public enum Operator
        {
            EqualTo = 0,
            GreaterThanOrEqualTo = 1,
            LessThanOrEqualTo = 2,
            //GreaterThan = 3,
            //LessThan = 4
        }


        public static bool Approx(double a, double b)
        {
            const double EPSILON = 1.0e-8;
            return Math.Abs(a - b) < EPSILON;
        }

        public static bool Approx(ClVariable clv, double b)
        {
            return Approx(clv.Value, b);
        }
       
    }
}