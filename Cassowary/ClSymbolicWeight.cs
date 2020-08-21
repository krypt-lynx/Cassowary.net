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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cassowary
{
    public class ClSymbolicWeight
    {
    

        public ClSymbolicWeight(double weight, double priority)
        {
            _priority = priority;
            _weight = weight;
        }

        /*
        protected ClSymbolicWeight(IEnumerable<double> weights)
            : this(weights.ToArray())
        {

        }*/

        protected virtual ClSymbolicWeight Clone()
        {
            return new ClSymbolicWeight(_weight, _priority);
        }

        public static ClSymbolicWeight operator *(ClSymbolicWeight clsw, double n)
        {
            return clsw.Times(n);
        }

        public static ClSymbolicWeight operator *(double n, ClSymbolicWeight clsw)
        {
            return clsw.Times(n);
        }

        public ClSymbolicWeight Times(double n)
        {
            return new ClSymbolicWeight(_weight * n, _priority);
            //return new ClSymbolicWeight(_values.Select(a => a * n).ToArray());
        }

        public static ClSymbolicWeight operator /(ClSymbolicWeight clsw, double n)
        {
            return clsw.DivideBy(n);
        }

        private ClSymbolicWeight DivideBy(double n)
        {
            // Assert(n != 0);

            return new ClSymbolicWeight(_weight / n, _priority);
            //return new ClSymbolicWeight(_values.Select(a => a / n).ToArray());
        }

     /*
        // TODO: comparison operators (<, <=, >, >=, ==)
        // not used
        public bool LessThan(ClSymbolicWeight clsw1)
        {
            // Assert(clsw1.CLevels == CLevels);

            for (var i = 0; i < _values.Count; i++)
            {
                if (_values[i] < clsw1._values[i])
                    return true;
                if (_values[i] > clsw1._values[i])
                    return false;
            }

            return false; // they are equal
        }

        // not used
        public bool LessThanOrEqual(ClSymbolicWeight clsw1)
        {
            // Assert(clsw1.CLevels == CLevels);

            for (var i = 0; i < _values.Count; i++)
            {
                if (_values[i] < clsw1._values[i])
                    return true;
                if (_values[i] > clsw1._values[i])
                    return false;
            }

            return true; // they are equal
        }

        // not used
        public bool Equal(ClSymbolicWeight clsw1)
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            return !_values.Where((t, i) => t != clsw1._values[i]).Any();
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        // not used
        public bool GreaterThan(ClSymbolicWeight clsw1)
        {
            return !LessThan(clsw1);
        }
        */

        public double AsDouble() // someone cheated in his thesis. Where is my symbolic order?
        {
            return Math.Pow(1000, _priority) * _weight;

            /*
            double sum = 0;
            double factor = 1;
            const double MULTIPLIER = 1000;
            int count = 3;
            for (var i = count - 1; i >= 0; i--)
            {
                double v = 0;
                if (i == 2 - _priority)
                {
                    v = _weight;
                }
                sum += v * factor;
                factor *= MULTIPLIER;
            }

            return sum;*/

            /*
            double sum = 0;
            double factor = 1;
            const double MULTIPLIER = 1000;

            for (var i = _values.Count - 1; i >= 0; i--)
            {
                sum += _values[i] * factor;
                factor *= MULTIPLIER;
            }

            return sum;
            */
        }

        public override string ToString()
        {
            return $"[{_priority}; {_weight}]";
        }

        /*public int CLevels
        {
            get { return _values.Count; }
        }*/

        //private readonly ReadOnlyCollection<double> _values;
        private double _weight;
        private double _priority;
    }
}