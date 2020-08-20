using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cassowary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
namespace CassowaryTests
{
    [TestClass]
    public class PythonExamples
    {
        private ClSimplexSolver _solver;

        [TestInitialize]
        public void Initialize()
        {
            _solver = new ClSimplexSolver();
        }

        [TestMethod]
        public void Playground()
        {
            //Implementation of this example: https://cassowary.readthedocs.org/en/latest/topics/examples.html#quadrilaterals

            var _0 = new ClPoint(10, 10);
            var _1 = new ClPoint(10, 200);
            var _2 = new ClPoint(200, 200);
            var _3 = new ClPoint(200, 10);

            var m0 = new ClPoint(0, 0);
            var m1 = new ClPoint(0, 0);
            var m2 = new ClPoint(0, 0);
            var m3 = new ClPoint(0, 0);

            //We don't want the points to move unless necessary
            _solver.AddPointStays(new[] {_0, _1, _2, _3});

            //Define the midpoints
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(m0.X, _0.X, _1.X, (m, a, b) => m == a * 0.5 + b * 0.5);
            _solver.AddConstraint(m0.Y, _0.Y, _1.Y, (m, a, b) => m == a * 0.5 + b * 0.5);

            _solver.AddConstraint(m1.X, _1.X, _2.X, (m, a, b) => m == a * 0.5 + b * 0.5);
            _solver.AddConstraint(m1.Y, _1.Y, _2.Y, (m, a, b) => m == a * 0.5 + b * 0.5);

            _solver.AddConstraint(m2.X, _2.X, _3.X, (m, a, b) => m == a * 0.5 + b * 0.5);
            _solver.AddConstraint(m2.Y, _2.Y, _3.Y, (m, a, b) => m == a * 0.5 + b * 0.5);

            _solver.AddConstraint(m3.X, _3.X, _0.X, (m, a, b) => m == a * 0.5 + b * 0.5);
            _solver.AddConstraint(m3.Y, _3.Y, _0.Y, (m, a, b) => m == a * 0.5 + b * 0.5);
// ReSharper restore CompareOfFloatsByEqualityOperator

            //Make sure left stays left and top stays top
            _solver.AddConstraint(_0.X, _2.X, (a, b) => a + 20 <= b);
            _solver.AddConstraint(_0.X, _3.X, (a, b) => a + 20 <= b);

            _solver.AddConstraint(_1.X, _2.X, (a, b) => a + 20 <= b);
            _solver.AddConstraint(_1.X, _3.X, (a, b) => a + 20 <= b);

            _solver.AddConstraint(_0.Y, _1.Y, (a, b) => a + 20 <= b);
            _solver.AddConstraint(_0.Y, _2.Y, (a, b) => a + 20 <= b);

            _solver.AddConstraint(_3.Y, _1.Y, (a, b) => a + 20 <= b);
            _solver.AddConstraint(_3.Y, _2.Y, (a, b) => a + 20 <= b);

            //Make sure all points stay in 500x500 convas
            _solver.AddConstraint(_0.X, a => a >= 0);
            _solver.AddConstraint(_0.Y, a => a >= 0);
            _solver.AddConstraint(_0.X, a => a <= 500);
            _solver.AddConstraint(_0.Y, a => a <= 500);

            Console.WriteLine(m0.X.Value + " " + m0.Y.Value);
            Console.WriteLine(m1.X.Value + " " + m1.Y.Value);
            Console.WriteLine(m2.X.Value + " " + m2.Y.Value);
            Console.WriteLine(m3.X.Value + " " + m3.Y.Value);

            _solver.BeginEdit(_2.X, _2.Y)
                .SuggestValue(_2.X, 300)
                .SuggestValue(_2.Y, 400)
                .EndEdit();

            Console.WriteLine(m0.X.Value + " " + m0.Y.Value);
            Console.WriteLine(m1.X.Value + " " + m1.Y.Value);
            Console.WriteLine(m2.X.Value + " " + m2.Y.Value);
            Console.WriteLine(m3.X.Value + " " + m3.Y.Value);

        }
    }
}*/
