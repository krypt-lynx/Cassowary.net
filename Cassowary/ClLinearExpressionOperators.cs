using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassowary
{
    public partial class ClLinearExpression : Cl
    {
        #region convertion
        public static implicit operator ClLinearExpression(ClAbstractVariable expr)
        {
            return new ClLinearExpression(expr);
        }
        public static implicit operator ClLinearExpression(int expr)
        {
            return new ClLinearExpression(expr);
        }
        public static implicit operator ClLinearExpression(double expr)
        {
            return new ClLinearExpression(expr);
        }
        #endregion

        #region operators
        public static ClLinearExpression operator +(ClLinearExpression expr)
        {
            return expr.Clone();
        }
        public static ClLinearExpression operator -(ClLinearExpression expr)
        {
            return expr.Clone().MultiplyMe(-1);
        }

        public static ClLinearExpression operator +(ClLinearExpression e1, ClLinearExpression e2)
        {
            return e1.Plus(e2);
        }
        public static ClLinearExpression operator -(ClLinearExpression e1, ClLinearExpression e2)
        {
            return e1.Minus(e2);
        }
        public static ClLinearExpression operator *(ClLinearExpression e1, ClLinearExpression e2)
        {
            return e1.Times(e2);
        }
        public static ClLinearExpression operator /(ClLinearExpression e1, ClLinearExpression e2)
        {
            return e1.Divide(e2);
        }
        #endregion

        #region comparision (return Constraints) 
        

        public static ClLinearInequality operator <(ClLinearExpression e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(ClLinearExpression e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }

        public static ClLinearInequality operator <=(ClLinearExpression e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(ClLinearExpression e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }

        public static ClLinearInequality operator <(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }

        public static ClLinearInequality operator <=(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <=(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }

        public static ClLinearInequality operator <(ClLinearExpression e1, double e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(double e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(ClLinearExpression e1, double e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <(double e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }

        public static ClLinearInequality operator <=(ClLinearExpression e1, double e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(double e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(ClLinearExpression e1, double e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <=(double e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }

        public static ClLinearInequality operator <(ClLinearExpression e1, int e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(int e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(ClLinearExpression e1, int e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <(int e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }

        public static ClLinearInequality operator <=(ClLinearExpression e1, int e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(int e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(ClLinearExpression e1, int e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <=(int e1, ClLinearExpression e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        
        
        public static ClLinearEquation operator ^(ClLinearExpression e1, ClLinearExpression e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }
        public static ClLinearEquation operator ^(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }
        public static ClLinearEquation operator ^(ClLinearExpression e1, double e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }
        public static ClLinearEquation operator ^(ClLinearExpression e1, int e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }
        public static ClLinearEquation operator ^(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }
        public static ClLinearEquation operator ^(double e1, ClLinearExpression e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }
        public static ClLinearEquation operator ^(int e1, ClLinearExpression e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }

        #endregion
    }
}
