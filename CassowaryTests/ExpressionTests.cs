using Cassowary_moddiff;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Cassowary_moddiffTests
{
    public class MemberAccessTestClass
    {
        public int Field;
        public int Property
        {
            get
            {
                return Field;
            }
        }
        
        public int[] IndexedProperty
        {
            get
            {
                return new int[] { Field };
            }
        }

        public ClVariable VarField;
        public ClVariable VarProperty
        {
            get
            {
                return VarField;
            }
        }
        public ClVariable[] VarIndexedProperty
        {
            get
            {
                return new ClVariable[] { VarField };
            }
        }

        public MemberAccessTestClass(int a)
        {
            Field = a;
        }

        public MemberAccessTestClass(ClVariable v)
        {
            VarField = v;
        }
    }

    [TestClass]
    public class ExpressionTests
    {
        private readonly ClSimplexSolver _solver = new ClSimplexSolver();

        [TestMethod]
        public void SingleParameterGreaterThanOrEqualToExpression()
        {
            _solver.AddConstraints(ClStrength.Required, new ClVariable("a") >= 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value >= 10);
        }

        [TestMethod]
        public void SingleParameterGreaterThanOrEqualToExpressionRuntimeVars()
        {
            ClVariable variable = new ClVariable(Guid.NewGuid().ToString());

            _solver.AddConstraints(variable >= 10);
            Assert.IsTrue(variable.Value >= 10);
        }

        [TestMethod]
        public void SingleParameterGreaterThanOrEqualToExpressionSwitched()
        {
            _solver.AddConstraints(10 >= new ClVariable("a"));
            Assert.IsTrue(10 >= ((ClVariable)_solver.GetVariable("a")).Value);
        }

        [TestMethod]
        public void SingleParameterLessThanOrEqualToExpression()
        {
            _solver.AddConstraints(new ClVariable("a") <= 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value <= 10);
        }

        [TestMethod]
        public void SingleParameterLessThanOrEqualToExpressionSwitched()
        {
            _solver.AddConstraints(10 <= new ClVariable("a"));
            Assert.IsTrue(10 <= ((ClVariable)_solver.GetVariable("a")).Value);
        }

        [TestMethod]
        public void SingleParameterEqualToExpression()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") ^ 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterEqualToExpressionSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(10 ^ new ClVariable("a"));
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearAdditionConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") + 3 ^ 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value + 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearAdditionConstraintSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(10 ^ new ClVariable("a") + 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value + 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearSubtractionConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") - 3 ^ 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value - 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearSubtractionConstraintSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(10 ^ new ClVariable("a") - 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value - 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearMultiplicationConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") * 3 ^ 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value * 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearMultiplicationConstraintSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(10 ^ new ClVariable("a") * 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value * 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearDivisionConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") / 3 ^ 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value / 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearDivisionConstraintSwitched()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(10 ^ new ClVariable("a") / 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value / 3 == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearConstraint()
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") / 3 * 2 + 1 - 3 ^ 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value / 3 * 2 + 1 - 3 == 10);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void SingleParameterLinearDivisionSwitched()
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(10 ^ new ClVariable("a") / 3 * 2 + 1 - 3);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value / 3 * 2 + 1 - 3 == 10);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterGreaterThanOrEqualToExpression()
        {
            _solver.AddConstraints(new ClVariable("a") >= new ClVariable("b"));
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value >= ((ClVariable)_solver.GetVariable("b")).Value);
        }

        [TestMethod]
        public void GreaterThanOrEqualToConstraint_ResolvesToAllowableValue()
        {
            ClVariable varA = new ClVariable("a");
            ClVariable varB = new ClVariable("b");

            _solver.AddConstraints(varA >= varB);
            Assert.IsTrue(varA.Value >= varB.Value);
        }

        [TestMethod]
        public void LessThanOrEqualToConstraint_ResolvesToAllowableValue()
        {
            _solver.AddConstraints(new ClVariable("a") <= new ClVariable("b"));
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value <= ((ClVariable)_solver.GetVariable("b")).Value);
        }

        [TestMethod]
        public void RangedConstraint_ResolvesToAllowableValue()
        {
            var a = new ClVariable("a");
            var b = new ClVariable("b");
            var c = new ClVariable("c");

            _solver.AddConstraints(a <= b, b <= c);

            var aa = (ClVariable)_solver.GetVariable("a");
            var bb = (ClVariable)_solver.GetVariable("b");
            var cc = (ClVariable)_solver.GetVariable("c");

            Assert.IsTrue(aa.Value <= bb.Value && bb.Value <= cc.Value);
        }

        [TestMethod]
        public void TwoParameterEqualityToExpression()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") ^ new ClVariable("b"));
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value == ((ClVariable)_solver.GetVariable("b")).Value);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterLinearAdditionConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") + new ClVariable("b") ^ 10);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value + ((ClVariable)_solver.GetVariable("b")).Value == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterLinearAdditionConstraint2()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") + 10 ^ new ClVariable("b"));
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value + ((ClVariable)_solver.GetVariable("b")).Value == 10);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterLinearConstraint()
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") * 10 + new ClVariable("b") >= 15);
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value * 10 + ((ClVariable)_solver.GetVariable("b")).Value >= 15);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void TwoParameterLinearConstraint2()
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(new ClVariable("a") * 10 + 15 >= new ClVariable("b"));
            Assert.IsTrue(((ClVariable)_solver.GetVariable("a")).Value * 10 + 15 >= ((ClVariable)_solver.GetVariable("b")).Value);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [TestMethod]
        public void MultiParameterAndConstraints()
        {
            var a = new ClVariable("a");
            var b = new ClVariable("b");
            var c = new ClVariable("c");
            var d = new ClVariable("d");

            _solver.AddConstraints(
                a >= b,
                b >= c,
                c >= d, 
                d * 2 + 3 - a <= 20);

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
        public void LambdaCallTest1()
        {
            ClVariable a_ = null;
            Func<string, ClVariable> a = (name) => a_ = new ClVariable(name);

            _solver.AddConstraints(4 ^ a("b"));

            Assert.IsTrue(Cl.Approx(a_, 4));
        }

        [TestMethod]
        public void LambdaCallTest2()
        {
            ClVariable a_ = null;
            Func<ClVariable> a = () => a_ = new ClVariable("a");

            _solver.AddConstraints(4 ^ a());

            Assert.IsTrue(Cl.Approx(a_, 4));
        }

        [TestMethod]
        public void MemberAccessTest1()
        {
            var a = new ClVariable("a");
            var b = new MemberAccessTestClass(10);
            
            _solver.AddConstraints(a ^ b.Field);

            Assert.IsTrue(Cl.Approx(a, 10));
        }


        [TestMethod]
        public void MemberAccessTest2()
        {
            var a = new ClVariable("a");
            var b = new MemberAccessTestClass(10);


            _solver.AddConstraints(a ^ b.Property);

            Assert.IsTrue(Cl.Approx(a, 10));
        }

        [TestMethod]
        public void ArrayAccess1()
        {
            var a = new ClVariable[] { new ClVariable("a"), new ClVariable("b") };

            _solver.AddConstraints(a[0] ^ 10);
            //_solver.AddConstraints(new ClLinearEquation(a[0], 10));

            Assert.IsTrue(Cl.Approx(a[0], 10));
        }

        [TestMethod]
        public void ArrayAccess2()
        {

            var a = new ClVariable("a");
            var b = new MemberAccessTestClass(a);


            _solver.AddConstraints(b.VarIndexedProperty[0] ^ 10);


            Assert.IsTrue(Cl.Approx(a, 10));
        }

        [TestMethod]
        public void MemberAccessTest3()
        {
            var a = new ClVariable("a");
            var b = new MemberAccessTestClass(10);
            
            _solver.AddConstraints(a ^ b.IndexedProperty[0]);


            Assert.IsTrue(Cl.Approx(a, 10));
        }
        
        [TestMethod]
        public void FieldMemberAccessExpression()
        {
// ReSharper disable ConvertToConstant.Local
            double field = 1;
// ReSharper restore ConvertToConstant.Local

            var variable = new ClVariable("a");

// ReSharper disable CompareOfFloatsByEqualityOperator
            _solver.AddConstraints(variable ^ field);
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
            _solver.AddConstraints(variable ^ field);
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
            _solver.AddConstraints(variable ^ -field / 2 * 3 + 4 - 2);
// ReSharper restore CompareOfFloatsByEqualityOperator
        }
        
        [TestMethod]
        [ExpectedException(typeof(CassowaryNonlinearExpressionException))]
        public void NonLinearExpressionThrowsException()
        {
            var x = new ClVariable("a");
            var y = new ClVariable("b");
       
            _solver.AddConstraints((new ClLinearExpression(x) / y) >= (new ClLinearExpression(y) / x));
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
            _solver.AddConstraints(((5 - doorHeightVariable) - windowHeight) ^ margin * 2);
// ReSharper restore CompareOfFloatsByEqualityOperator

            Assert.AreEqual(5 - 2 - 1, margin.Value * 2);
        }
    }
}
