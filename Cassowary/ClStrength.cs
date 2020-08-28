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

namespace Cassowary
{
    public class ClStrength
    {
        private ClStrength(string name, ClSymbolicWeight symbolicWeight)
        {
            Name = name;
            SymbolicWeight = symbolicWeight;
        }

        private ClStrength(string name, double weight, double priority)
            : this(name, new ClSymbolicWeight(weight, priority))
        {
        }

        public bool IsRequired
        {
            get { return (this == Required); }
        }

        public ClSymbolicWeight SymbolicWeight { get; private set; }

        public string Name { get; private set; }

        public override string ToString()
        {
            if (IsRequired)
                return Name;
            else
                return string.Format("{0}:{1}", Name, SymbolicWeight);
        }

        /// <summary>
        /// Highest possible constraint priority
        /// </summary>
        /// <remarks>
        /// Solver will throw exception if cannot be satisfied
        /// </remarks>
        public static ClStrength Required { get; } = new ClStrength("required", 1.0, 100);

        /// <summary>
        /// Strong constraint priority
        /// </summary>
        public static ClStrength Strong { get; } = new ClStrength("strong", 1.0, 2);

        /// <summary>
        /// Medium constraint priority
        /// </summary>
        public static ClStrength Medium { get; } = new ClStrength("medium", 1.0, 1);

        /// <summary>
        /// Weak
        /// </summary>
        public static ClStrength Weak { get; } = new ClStrength("weak", 1.0, 0);

        public static ClStrength Default { get; } = Strong;
    }
}