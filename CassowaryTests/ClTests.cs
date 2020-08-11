﻿using Cassowary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace CassowaryTests
{
    [TestClass]
    public class ClTests
    {
        private readonly Random _rnd;
        private readonly ClSimplexSolver _solver = new ClSimplexSolver();

        public ClTests()
        {
            _rnd = new Random(123456789);
        }

        [TestMethod]
        public void Playground()
        {
            var x = new ClVariable("x");
            _solver.AddConstraint(x, a => a < -1);

            Assert.IsTrue(x.Value < -1);
        }

        [TestMethod]
        public void AddingConstraintToMakeTwoVariablesEqualMakesVariablesEqual()
        {
            var x = new ClVariable("x", 167);
            var y = new ClVariable("y", 2);

            var eq = new ClLinearEquation(x, new ClLinearExpression(y));
            _solver.AddConstraint(eq);

            Assert.AreEqual(x.Value, y.Value);
        }

        [TestMethod]
        public void AddingValueStayConstraintMakesValueNotChange()
        {
            var x = new ClVariable("x", 5);

            _solver.AddStay(x);

            Assert.IsTrue(Cl.Approx(x, 5));
        }

        [TestMethod]
        public void AddingAndRemovingConstraintsUpdatesValue()
        {
            var x = new ClVariable("x");

            _solver.AddConstraint(new ClLinearEquation(x, 100, ClStrength.Weak));

            var c10 = new ClLinearInequality(x, Cl.Operator.LessThanOrEqualTo, 10.0);
            var c20 = new ClLinearInequality(x, Cl.Operator.LessThanOrEqualTo, 20.0);

            _solver.AddConstraint(c10).AddConstraint(c20);
            Assert.IsTrue(Cl.Approx(x, 10.0));

            _solver.RemoveConstraint(c10);
            Assert.IsTrue(Cl.Approx(x, 20.0));

            _solver.RemoveConstraint(c20);
            Assert.IsTrue(Cl.Approx(x, 100.0));

            var c10Again = new ClLinearInequality(x, Cl.Operator.LessThanOrEqualTo, 10.0);

            _solver.AddConstraint(c10).AddConstraint(c10Again);
            Assert.IsTrue(Cl.Approx(x, 10.0));

            _solver.RemoveConstraint(c10);
            Assert.IsTrue(Cl.Approx(x, 10.0));

            _solver.RemoveConstraint(c10Again);
            Assert.IsTrue(Cl.Approx(x, 100.0));
        }

        [TestMethod]
        public void AddDelete2()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");

            _solver
              .AddConstraint(new ClLinearEquation(x, 100.0, ClStrength.Weak))
              .AddConstraint(new ClLinearEquation(y, 120.0, ClStrength.Strong));

            var c10 = new ClLinearInequality(x, Cl.Operator.LessThanOrEqualTo, 10.0);
            var c20 = new ClLinearInequality(x, Cl.Operator.LessThanOrEqualTo, 20.0);

            _solver
              .AddConstraint(c10)
              .AddConstraint(c20);
            Assert.IsTrue(Cl.Approx(x, 10.0));
            Assert.IsTrue(Cl.Approx(y, 120.0));

            _solver.RemoveConstraint(c10);
            Assert.IsTrue(Cl.Approx(x, 20.0));
            Assert.IsTrue(Cl.Approx(y, 120.0));
            
            var cxy = new ClLinearEquation(Cl.Times(2.0, x), y);
            _solver.AddConstraint(cxy);
            Assert.IsTrue(Cl.Approx(x, 20.0));
            Assert.IsTrue(Cl.Approx(y, 40.0));

            _solver.RemoveConstraint(c20);
            Assert.IsTrue(Cl.Approx(x, 60.0));
            Assert.IsTrue(Cl.Approx(y, 120.0));

            _solver.RemoveConstraint(cxy);
            Assert.IsTrue(Cl.Approx(x, 100.0));
            Assert.IsTrue(Cl.Approx(y, 120.0));
        }

        [TestMethod]
        public void Casso1()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");


            _solver
              .AddConstraint(new ClLinearInequality(x, Cl.Operator.LessThanOrEqualTo, y))
              .AddConstraint(new ClLinearEquation(y, Cl.Plus(x, 3.0)))
              .AddConstraint(new ClLinearEquation(x, 10.0, ClStrength.Weak))
              .AddConstraint(new ClLinearEquation(y, 10.0, ClStrength.Weak));

            Assert.IsTrue(
                (Cl.Approx(x, 10.0) && Cl.Approx(y, 13.0)) ||
                (Cl.Approx(x, 7.0) && Cl.Approx(y, 10.0))
            );
        }

        [TestMethod]
        [ExpectedException(typeof(CassowaryRequiredFailureException))]
        public void Inconsistent1()
        {
            var x = new ClVariable("x");


            _solver
                .AddConstraint(new ClLinearEquation(x, 10.0))
                .AddConstraint(new ClLinearEquation(x, 5.0));
        }

        [TestMethod]
        [ExpectedException(typeof(CassowaryRequiredFailureException))]
        public void Inconsistent2()
        {
            var x = new ClVariable("x");


            _solver
                .AddConstraint(new ClLinearInequality(x, Cl.Operator.GreaterThanOrEqualTo, 10.0))
                .AddConstraint(new ClLinearInequality(x, Cl.Operator.LessThanOrEqualTo, 5.0));
        }

        [TestMethod]
        public void InconsistentConstraintsAreCorrectlyResolvedAccordingToStrength()
        {
            var x = new ClVariable("x");


// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(x, a => a == 10, ClStrength.Strong);
            _solver.AddConstraint(x, a => a == 5, ClStrength.Medium);
// ReSharper restore CompareOfFloatsByEqualityOperator

            Assert.IsTrue(Math.Abs(x.Value - 10.0) < float.Epsilon);
        }

        [TestMethod]
        public void Multiedit()
        {
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var w = new ClVariable("w");
            var h = new ClVariable("h");


            var e1 = _solver
                .AddStay(x)
                .AddStay(y)
                .AddStay(w)
                .AddStay(h)

                .BeginEdit(x, y)

                .SuggestValue(x, 10)
                .SuggestValue(y, 20)
                .Resolve();

            Assert.IsTrue(Cl.Approx(x, 10));
            Assert.IsTrue(Cl.Approx(y, 20));
            Assert.IsTrue(Cl.Approx(w, 0));
            Assert.IsTrue(Cl.Approx(h, 0));

            _solver
                .BeginEdit(w, h)
                .SuggestValue(w, 30)
                .SuggestValue(h, 40)
                .EndEdit();

            Assert.IsTrue(Cl.Approx(x, 10));
            Assert.IsTrue(Cl.Approx(y, 20));
            Assert.IsTrue(Cl.Approx(w, 30));
            Assert.IsTrue(Cl.Approx(h, 40));

            e1
                .SuggestValue(x, 50)
                .SuggestValue(y, 60)
                .EndEdit();

            Assert.IsTrue(Cl.Approx(x, 50));
            Assert.IsTrue(Cl.Approx(y, 60));
            Assert.IsTrue(Cl.Approx(w, 30));
            Assert.IsTrue(Cl.Approx(h, 40));
        }

        [TestMethod]
        [ExpectedException(typeof(CassowaryRequiredFailureException))]
        public void Inconsistent3()
        {
            var w = new ClVariable("w");
            var x = new ClVariable("x");
            var y = new ClVariable("y");
            var z = new ClVariable("z");


            _solver
                .AddConstraint(new ClLinearInequality(w, Cl.Operator.GreaterThanOrEqualTo, 10.0))
                .AddConstraint(new ClLinearInequality(x, Cl.Operator.GreaterThanOrEqualTo, w))
                .AddConstraint(new ClLinearInequality(y, Cl.Operator.GreaterThanOrEqualTo, x))
                .AddConstraint(new ClLinearInequality(z, Cl.Operator.GreaterThanOrEqualTo, y))
                .AddConstraint(new ClLinearInequality(z, Cl.Operator.GreaterThanOrEqualTo, 8.0))
                .AddConstraint(new ClLinearInequality(z, Cl.Operator.LessThanOrEqualTo, 4.0));
        }

        [TestMethod]
        [ExpectedException(typeof(CassowaryConstraintNotFoundException))]
        public void DeleteNonAddedConstraintThrowsConstraintNotFoundException()
        {

            _solver.RemoveConstraint(new ClLinearInequality(new ClVariable("a"), Cl.Operator.GreaterThanOrEqualTo, new ClVariable("b")));
        }

        [TestMethod]
        public void AddDel()
        {
            const int nCns = 450;
            const int nVars = 450;
            const int nResolves = 5000;

            var timer = new Stopwatch();
            const double ineqProb = 0.12;
            const int maxVars = 3;

            Console.WriteLine("starting timing test. nCns = " + nCns +
                ", nVars = " + nVars + ", nResolves = " + nResolves);

            timer.Start();
            

            var rgpclv = new ClVariable[nVars];
            for (var i = 0; i < nVars; i++)
            {
                rgpclv[i] = new ClVariable(i, "x");
                _solver.AddStay(rgpclv[i]);
            }

            var rgpcns = new ClConstraint[nCns];
            int j;
            for (j = 0; j < nCns; j++)
            {
                // number of variables in this constraint
                var nvs = RandomInRange(1, maxVars);
                var expr = new ClLinearExpression(UniformRandomDiscretized() * 20.0 - 10.0);
                int k;
                for (k = 0; k < nvs; k++)
                {
                    var coeff = UniformRandomDiscretized() * 10 - 5;
                    var iclv = (int)(UniformRandomDiscretized() * nVars);
                    expr.AddExpression(Cl.Times(rgpclv[iclv], coeff));
                }
                if (UniformRandomDiscretized() < ineqProb)
                {
                    rgpcns[j] = new ClLinearInequality(expr);
                }
                else
                {
                    rgpcns[j] = new ClLinearEquation(expr);
                }
            }

            Console.WriteLine("done building data structures");
            Console.WriteLine("time = " + timer.Elapsed);
            timer.Start();
            var cExceptions = 0;
            for (j = 0; j < nCns; j++)
            {
                // add the constraint -- if it's incompatible, just ignore it
                try
                {
                    _solver.AddConstraint(rgpcns[j]);
                }
                catch (CassowaryRequiredFailureException)
                {
                    cExceptions++;
                    rgpcns[j] = null;
                }
            }
            Console.WriteLine("done adding constraints [" + cExceptions + " exceptions]");
            Console.WriteLine("time = " + timer.Elapsed + "\n");
            timer.Start();

            var e1Index = (int)(UniformRandomDiscretized() * nVars);
            var e2Index = (int)(UniformRandomDiscretized() * nVars);

            Console.WriteLine("indices " + e1Index + ", " + e2Index);

            var edit1 = new ClEditConstraint(rgpclv[e1Index], ClStrength.Strong);
            var edit2 = new ClEditConstraint(rgpclv[e2Index], ClStrength.Strong);

            _solver
              .AddConstraint(edit1)
              .AddConstraint(edit2);

            Console.WriteLine("done creating edit constraints -- about to start resolves");
            Console.WriteLine("time = " + timer.Elapsed + "\n");
            timer.Start();

            //for (var m = 0; m < nResolves; m++)
            //{
            //    _solver.Resolve(rgpclv[e1Index].Value * 1.001,
            //                   rgpclv[e2Index].Value * 1.001);
            //}

            Console.WriteLine("done resolves -- now removing constraints");
            Console.WriteLine("time = " + timer.Elapsed + "\n");

            _solver.RemoveConstraint(edit1);
            _solver.RemoveConstraint(edit2);

            timer.Start();

            for (j = 0; j < nCns; j++)
            {
                if (rgpcns[j] != null)
                {
                    _solver.RemoveConstraint(rgpcns[j]);
                }
            }

            Console.WriteLine("done removing constraints and AddDel timing test");
            Console.WriteLine("time = " + timer.Elapsed + "\n");

            timer.Start();
        }

        private double UniformRandomDiscretized()
        {
            double n = Math.Abs(_rnd.Next());
            return n / int.MaxValue;
        }

        private int RandomInRange(int low, int high)
        {
            return (int)UniformRandomDiscretized() * (high - low) + low;
        }
    }
}
