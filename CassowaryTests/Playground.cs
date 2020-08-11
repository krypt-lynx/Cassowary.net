using System;
using System.Diagnostics;
using Cassowary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CassowaryTests
{
    [TestClass]
    public class Playground
    {
        [TestMethod]
        public void Z3()
        {
            Stopwatch w = new Stopwatch();

            w.Restart();
            var solver = new ClSimplexSolver() {
                AutoSolve = false
            };
            Console.WriteLine("cons {0}ms", w.ElapsedMilliseconds);

            w.Restart();
            solver.AddConstraint(new ClVariable("x"), new ClVariable("y"), new ClVariable("z"), (x, y, z) => 
                x > 1
                && y == x + 1
                && y < 3
                && z == x + y * 3
            );
            Console.WriteLine("setup {0}ms", w.ElapsedMilliseconds);

            w.Restart();
            solver = solver.Solve();
            Console.WriteLine("solve {0}ms", w.ElapsedMilliseconds);

            w.Restart();
            Console.WriteLine("x " + ((ClVariable)solver.GetVariable("x")).Value);
            Console.WriteLine("y " + ((ClVariable)solver.GetVariable("y")).Value);
            Console.WriteLine("z " + ((ClVariable)solver.GetVariable("z")).Value);
            Console.WriteLine("read {0}ms", w.ElapsedMilliseconds);
        }
    }
}
