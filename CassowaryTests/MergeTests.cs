using Cassowary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassowaryTests
{
    [TestClass]
    public class MergeTests
    {
        [TestMethod]
        public void MergeTest()
        {

            var s1 = new ClSimplexSolver();
            s1.AutoSolve = false;


            var Row_L = new ClVariable("Row_L");
            var Row_R = new ClVariable("Row_R");
            var Row_T = new ClVariable("Row_T");
            var Row_B = new ClVariable("Row_B");

            var Row_W = new ClVariable("Row_W");
            var Row_H = new ClVariable("Row_H");

            s1.AddConstraint(new ClStayConstraint(Row_L, 0, ClStrength.Required));
            s1.AddConstraint(new ClStayConstraint(Row_R, 230, ClStrength.Required));
            s1.AddConstraint(new ClStayConstraint(Row_T, 50, ClStrength.Required));
            s1.AddConstraint(ClStrength.Required, Row_T + Row_H ^ Row_B);



            var Item_L = new ClVariable("Item_L");
            var Item_R = new ClVariable("Item_R");
            var Item_T = new ClVariable("Item_T");
            var Item_B = new ClVariable("Item_B");

            var Item_W = new ClVariable("Item_W");
            var Item_H = new ClVariable("Item_H");


            var Label_L = new ClVariable("Label_L");
            var Label_R = new ClVariable("Label_R");
            var Label_T = new ClVariable("Label_T");
            var Label_B = new ClVariable("Label_B");

            var Label_W = new ClVariable("Label_W");
            var Label_H = new ClVariable("Label_H");

            var s2 = new ClSimplexSolver();
            //var s2 = s1;
            s2.AutoSolve = false;

            s2.AddConstraint(Label_L ^ Item_L);
            s2.AddConstraint(Label_R ^ Item_R);
            s2.AddConstraint(Label_T ^ Item_T);
            s2.AddConstraint(Label_B ^ Item_B);


            s2.AddConstraint(Item_H ^ 50);
            s2.AddConstraint(ClStrength.Required, Item_T + Item_H ^ Item_B);

            s1.MergeWith(s2);

            s1.AddConstraint(Item_L ^ Row_L);
            s1.AddConstraint(Item_R ^ Row_R);
            s1.AddConstraint(Item_T ^ Row_T);
            s1.AddConstraint(Item_B ^ Row_B);

            s1.Solve();

            Assert.AreEqual(50, Row_H.Value);


            ShearConstraints(s1, new ClVariable[] {
                Item_L, Item_R, Item_T, Item_B, Item_W, Item_H,
                Label_L, Label_R, Label_T, Label_B, Label_W, Label_H,
            });


            s1.AddConstraint(ClStrength.Strong, Row_H ^ 20);


            s1.Solve();

            Assert.AreEqual(20, Row_H.Value);
            //Console.WriteLine();
            //Console.WriteLine(Row_H);
           // Console.ReadKey();

        }

        static private List<ClConstraint> ShearConstraints(ClSimplexSolver solver, IEnumerable<ClVariable> anchors)
        {
            var cns = solver.AllConstraints();
            var detachedAnchors = anchors;
            var movedConstraints = new List<ClConstraint>();

            foreach (var cn in cns)
            {
                bool hasDetached = false;
                bool hasAttached = false;

                foreach (var var in cn.Expression.Terms.Keys)
                {
                    if (detachedAnchors.Contains(var))
                    {
                        hasDetached = true;
                    }
                    else
                    {
                        hasAttached = true;
                    }

                    if (hasAttached && hasDetached)
                    {
                        break;
                    }
                }

                if (hasDetached)
                {
                    solver.RemoveConstraint(cn);
                    if (!hasAttached)
                    {
                        movedConstraints.Add(cn);
                    }
                }
            }

            foreach (var var in detachedAnchors)
            {
                solver.RemoveVariable(var);
            }

            return movedConstraints;
        }

    }
}
