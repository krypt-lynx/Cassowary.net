using Cassowary_moddiff;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassowary_moddiffTests
{
    
    [TestClass]
    public class MergeTests
    {
        [TestMethod]
        public void MergeTest1()
        {
            GC.Collect();
            long before = GC.GetTotalMemory(true);

            var s1 = new ClSimplexSolver();
            var s2 = new ClSimplexSolver();

            var a = new ClVariable("a");
            var b = new ClVariable("b");
            var c = new ClVariable("c");

            s1.AddConstraint(a ^ 40);
            s2.AddConstraint(b ^ c);

            s1.MergeWith(s2);
            // FixMe: Simplex contains 2 objectives now! (and somehow everything works)

            s1.AddConstraint(a ^ b);

            GC.Collect();
            var used = GC.GetTotalMemory(true) - before;
            Console.WriteLine($"Memory used: {used}");

            Assert.IsTrue(Cl.Approx(c, 40));
        }


        [TestMethod]
        public void MergeTest2()
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
            s1.AddConstraint(Row_T + Row_H ^ Row_B, ClStrength.Required);

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
            s2.AddConstraint(Item_T + Item_H ^ Item_B, ClStrength.Required);

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


            s1.AddConstraint(Row_H ^ 20, ClStrength.Strong);


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

        [TestMethod]
        public void Hugging()
        {
            var s1 = new ClSimplexSolver();
            var s2 = new ClSimplexSolver();

            s1.AutoSolve = false;
            s2.AutoSolve = false;
            //var s2 = s1;

            var CElement_2_T = new ClVariable("CElement_2_T");
            var CElement_buttonsPanel_1_T = new ClVariable("CElement_buttonsPanel_1_T");
            var CElement_3_T = new ClVariable("CElement_3_T");
            var CButton_4_T = new ClVariable("CButton_4_T");
            var CButton_4_H = new ClVariable("CButton_4_H");
            var CButton_4_B = new ClVariable("CButton_4_B");
            var CElement_2_B = new ClVariable("CElement_2_B");
            var CCheckBox_5_T = new ClVariable("CCheckBox_5_T");
            var CCheckBox_5_H = new ClVariable("CCheckBox_5_H");
            var CCheckBox_5_B = new ClVariable("CCheckBox_5_B");
            var CElement_3_B = new ClVariable("CElement_3_B");
            var CElement_buttonsPanel_1_B = new ClVariable("CElement_buttonsPanel_1_B");
            var CWindowRoot_0_T = new ClVariable("CWindowRoot_0_T");
            var CWindowRoot_0_B = new ClVariable("CWindowRoot_0_B");


            s2.AddConstraint(CElement_2_T ^ CElement_buttonsPanel_1_T);
            s2.AddConstraint(CElement_3_T ^ CElement_buttonsPanel_1_T);
            s2.AddConstraint(CElement_2_T ^ CButton_4_T);
            s2.AddConstraint(CButton_4_H ^ 60);
            s2.AddConstraint(CButton_4_B ^ CElement_2_B);
            s2.AddConstraint(CElement_3_T ^ CCheckBox_5_T);
            s2.AddConstraint(CCheckBox_5_H ^ 30);
            s2.AddConstraint(CCheckBox_5_B ^ CElement_3_B);
            s2.AddConstraint(CElement_buttonsPanel_1_B >= CElement_2_B);
            s2.AddConstraint(CElement_buttonsPanel_1_B >= CElement_3_B);
            s2.AddConstraint(CWindowRoot_0_T ^ CElement_buttonsPanel_1_T);
            s2.AddConstraint(CWindowRoot_0_B ^ CElement_buttonsPanel_1_B);




            s1.MergeWith(s2);

            s1.AddConstraint(new ClStayConstraint(CWindowRoot_0_T, 0));

            s1.Solve();
            s1.AddConstraint(CButton_4_T + CButton_4_H ^ CButton_4_B);
            s1.AddConstraint(CCheckBox_5_T + CCheckBox_5_H ^ CCheckBox_5_B);

            Assert.IsTrue(CButton_4_H.Value == 60);

            
//s2 add    
//strong:[2; 1]:1 {[CElement_2_T:0] = [CElement_buttonsPanel_1_T:0]}
//strong:[2; 1]:1 {[CElement_3_T:0] = [CElement_buttonsPanel_1_T:0]}
//strong:[2; 1]:1 {[CElement_2_T:0] = [CButton_4_T:0]}
//strong:[2; 1]:1 {[CButton_4_H:60] = 60}
//strong:[2; 1]:1 {[CButton_4_B:60] = [CElement_2_B:60]}
//strong:[2; 1]:1 {[CElement_3_T:0] = [CCheckBox_5_T:0]}
//strong:[2; 1]:1 {[CCheckBox_5_H:30] = 30}
//strong:[2; 1]:1 {[CCheckBox_5_B:30] = [CElement_3_B:30]}
//medium:[1; 1]:1 {[CElement_buttonsPanel_1_B:60] ≥ [CElement_2_B:60]}
//medium:[1; 1]:1 {[CElement_buttonsPanel_1_B:60] ≥ [CElement_3_B:30]}
//strong:[2; 1]:1 {[CWindowRoot_0_T:0] = [CElement_buttonsPanel_1_T:0]}
//strong:[2; 1]:1 {[CWindowRoot_0_B:60] = [CElement_buttonsPanel_1_B:60]}
//s1.merge(s2)
//s1 add
//stayrequired:1 {[CWindowRoot_0_T:0] = 0}
//required:1 {[CButton_4_T:0] + [CButton_4_H:60] = [CButton_4_B:60]}
//required:1 {[CCheckBox_5_T:0] + [CCheckBox_5_H:30] = [CCheckBox_5_B:30]}
            
        }
    }


}
