using Cassowary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CassowaryTests
{
    [TestClass]
    public class ExpressionTests
    {
        private readonly ClSimplexSolver _solver = new ClSimplexSolver();

        [TestMethod]
        public void SingleParameterGreaterThanOrEqualToExpression()
        {
            _solver.AddConstraint(a => a >= 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value >= 10);
        }

        [TestMethod]
        public void SingleParameterGreaterThanOrEqualToExpressionRuntimeVars()
        {
            ClVariable variable = new ClVariable(Guid.NewGuid().ToString());

            _solver.AddConstraint(variable, a => a >= 10);
            Assert.IsTrue(variable.Value >= 10);
        }

        [TestMethod]
        public void SingleParameterGreaterThanOrEqualToExpressionSwitched()
        {
            _solver.AddConstraint(a => 10 >= a);
            Assert.IsTrue(10 >= ((ClVariable)_solver.GetVariable("a")).Value);
        }

        [TestMethod]
        public void SingleParameterLessThanOrEqualToExpression()
        {
            _solver.AddConstraint(a => a <= 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value <= 10);
        }

        [TestMethod]
        public void SingleParameterLessThanOrEqualToExpressionSwitched()
        {
            _solver.AddConstraint(a => 10 <= a);
            Assert.IsTrue(10 <= ((ClVariable)_solver.GetVariable("a")).Value);
        }

        [TestMethod]
        public void SingleParameterEqualToExpression()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => a == 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterEqualToExpressionSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => 10 == a);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearAdditionConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => a + 3 == 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value + 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearAdditionConstraintSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => 10 == a + 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value + 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearSubtractionConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => a - 3 == 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value - 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearSubtractionConstraintSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => 10 == a - 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value - 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearMultiplicationConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => a * 3 == 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value * 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearMultiplicationConstraintSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => 10 == a * 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value * 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearDivisionConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => a / 3 == 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value / 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearDivisionConstraintSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => 10 == a / 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value / 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearConstraint()
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => a / 3 * 2 + 1 - 3 == 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value / 3 * 2 + 1 - 3 == 10);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearDivisionSwitched()
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(a => 10 == a / 3 * 2 + 1 - 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value / 3 * 2 + 1 - 3 == 10);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterGreaterThanOrEqualToExpression()
        {
            _solver.AddConstraint((a, b) => a >= b);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value >= ((ClVariable)_solver.GetVariable("b")).Value);
        }

        [TestMethod]
        public void GreaterThanOrEqualToConstraint_ResolvesToAllowableValue()
        {
            ClVariable varA = new ClVariable("a");
            ClVariable varB = new ClVariable("b");

            _solver.AddConstraint(varA, varB, (a, b) => a >= b);
            Assert.IsTrue(varA.Value >= varB.Value);
        }

        [TestMethod]
        public void LessThanOrEqualToConstraint_ResolvesToAllowableValue()
        {
            _solver.AddConstraint((a, b) => a <= b);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value <= ((ClVariable)_solver.GetVariable("b")).Value);
        }

        [TestMethod]
        public void RangedConstraint_ResolvesToAllowableValue()
        {
            _solver.AddConstraint((a, b, c) => a <= b && b <= c);

            var aa = (ClVariable)_solver.GetVariable("a");
            var bb = (ClVariable)_solver.GetVariable("b");
            var cc = (ClVariable)_solver.GetVariable("c");

            Assert.IsTrue(aa.Value <= bb.Value && bb.Value <= cc.Value);
        }

        [TestMethod]
        public void TwoParameterEqualityToExpression()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint((a, b) => a == b);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value == ((ClVariable)_solver.GetVariable("b")).Value);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterLinearAdditionConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint((a, b) => a + b == 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value + ((ClVariable)_solver.GetVariable("b")).Value == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterLinearAdditionConstraint2()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint((a, b) => a + 10 == b);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value + ((ClVariable)_solver.GetVariable("b")).Value == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterLinearConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint((a, b) => a * 10 + b >= 15);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value * 10 + ((ClVariable)_solver.GetVariable("b")).Value >= 15);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterLinearConstraint2()
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint((a, b) => a * 10 + 15 >= b);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value * 10 + 15 >= ((ClVariable)_solver.GetVariable("b")).Value);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void MultiParameterAndConstraints()
        {
            _solver.AddConstraint((a, b, c, d) => a >= b && b >= c && c >= d && d * 2 + 3 - a <= 20);

            var aa = ((ClVariable)_solver.GetVariable("a")).Value;
            var bb = ((ClVariable)_solver.GetVariable("b")).Value;
            var cc = ((ClVariable)_solver.GetVariable("c")).Value;
            var dd = ((ClVariable)_solver.GetVariable("d")).Value;

            Assert.IsTrue(aa >= bb);
            Assert.IsTrue(bb >= cc);
            Assert.IsTrue(cc >= dd);
            Assert.IsTrue(dd * 2 + 3 - aa <= 20);
        }

        [TestMethod]
        public void FieldMemberAccessExpression()
        {
// ReSharper disable ConvertToConstant.Local
            double field = 1;
// ReSharper restore ConvertToConstant.Local

            var variable = new ClVariable("a");

// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(variable, a => a == field);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void ConvertExpression()
        {
// ReSharper disable ConvertToConstant.Local
            float field = 1;
// ReSharper restore ConvertToConstant.Local

            var variable = new ClVariable("a");

// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(variable, a => a == field);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void FieldMemberAccessWithArithmeticExpression()
        {
// ReSharper disable ConvertToConstant.Local
            float field = 1;
// ReSharper restore ConvertToConstant.Local

            var variable = new ClVariable("a");

// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(variable, a => a == -field / 2 * 3 + 4 - 2);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        [ExpectedException(typeof(CassowaryNonlinearExpressionException))]
        public void NonLinearExpressionThrowsException()
        {
            var a = new ClVariable("a");
            var b = new ClVariable("b");

            _solver.AddConstraint(a, b, (x, y) => (x / y) >= (y / x));
        }

        [TestMethod]
        public void Playground()
        {
            var windowHeight = new ClVariable(1);
            _solver.AddStay(windowHeight);
            var doorHeightVariable = new ClVariable(2);
            _solver.AddStay(doorHeightVariable);

            var margin = new ClVariable("margin");
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraint(windowHeight, doorHeightVariable, margin, (wh, dh, wm) => ((5 - dh) - wh) == wm * 2);
// ReSharper restore CompareOfFloatsByEqualityOperator

            Assert.AreEqual(5 - 2 - 1, margin.Value * 2);
        }
    }
}
