using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassowary
{
    public abstract partial class ClConstraint
    {
        /*
        public static IEnumerable<ClConstraint> operator &(ClConstraint e1, ClConstraint e2)
        {
            yield return e1;
            yield return e2;
        }
        public static IEnumerable<ClConstraint> operator &(IEnumerable<ClConstraint> e1, ClConstraint e2)
        {
            foreach (var e in e1)
            {
                yield return e;
            }
            yield return e2;
        }
        public static IEnumerable<ClConstraint> operator &(ClConstraint e1, IEnumerable<ClConstraint> e2)
        {
            yield return e1;
            foreach (var e in e2)
            {
                yield return e;
            }
        }*/
    }
}
