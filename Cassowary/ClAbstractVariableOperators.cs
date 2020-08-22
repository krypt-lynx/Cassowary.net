using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cassowary.Cl;

namespace Cassowary
{
    public abstract partial class ClAbstractVariable
    {
        // say hello to combinatoric explosion

        public static ClLinearExpression operator +(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }
        public static ClLinearExpression operator +(ClAbstractVariable e1, double e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }
        public static ClLinearExpression operator +(double e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }
        public static ClLinearExpression operator +(ClAbstractVariable e1, int e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }
        public static ClLinearExpression operator +(int e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }


        public static ClLinearExpression operator -(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }
        public static ClLinearExpression operator -(ClAbstractVariable e1, double e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }
        public static ClLinearExpression operator -(double e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }
        public static ClLinearExpression operator -(ClAbstractVariable e1, int e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }
        public static ClLinearExpression operator -(int e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }


        // no variable*variable: exlressions are linear and can have only x^1 components
        public static ClLinearExpression operator *(ClAbstractVariable e1, double e2)
        {
            return new ClLinearExpression(e1).MultiplyMe(e2);
        }
        public static ClLinearExpression operator *(double e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e2).MultiplyMe(e1);
        }
        public static ClLinearExpression operator *(ClAbstractVariable e1, int e2)
        {
            return new ClLinearExpression(e1).MultiplyMe(e2);
        }
        public static ClLinearExpression operator *(int e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e2).MultiplyMe(e1);
        }


        // no x/variable and variable/variable: exlressions are linear and can have only x^1 components
        public static ClLinearExpression operator /(ClAbstractVariable e1, int e2)
        {
            return new ClLinearExpression(e1).DivideMe(e2);
        }
        public static ClLinearExpression operator /(ClAbstractVariable e1, double e2)
        {
            return new ClLinearExpression(e1).DivideMe(e2);
        }


        #region comparision (return Constraints) 


        public static ClLinearInequality operator <(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }

        public static ClLinearInequality operator <=(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }

        public static ClLinearInequality operator <(ClAbstractVariable e1, double e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(double e1, ClAbstractVariable e2)
        {
            return e2 < e1;
        }
        public static ClLinearInequality operator >(ClAbstractVariable e1, double e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <(double e1, ClAbstractVariable e2)
        {
            return e2 > e1;
        }

        public static ClLinearInequality operator <=(ClAbstractVariable e1, double e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(double e1, ClAbstractVariable e2)
        {
            return e2 <= e1;
        }
        public static ClLinearInequality operator >=(ClAbstractVariable e1, double e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <=(double e1, ClAbstractVariable e2)
        {
            return e2 >= e1;
        }

        public static ClLinearInequality operator <(ClAbstractVariable e1, int e2)
        {
            return new ClLinearInequality(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >(int e1, ClAbstractVariable e2)
        {
            return e2 < e1;
        }
        public static ClLinearInequality operator >(ClAbstractVariable e1, int e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <(int e1, ClAbstractVariable e2)
        {
            return e2 > e1;
        }

        public static ClLinearInequality operator <=(ClAbstractVariable e1, int e2)
        {
            return new ClLinearInequality(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator >=(int e1, ClAbstractVariable e2)
        {
            return e2 <= e1;
        }
        public static ClLinearInequality operator >=(ClAbstractVariable e1, int e2)
        {
            return new ClLinearInequality(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        public static ClLinearInequality operator <=(int e1, ClAbstractVariable e2)
        {
            return e2 >= e1;
        }

        
        public static ClLinearEquation operator ^(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }      
        public static ClLinearEquation operator ^(ClAbstractVariable e1, double e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }
        public static ClLinearEquation operator ^(ClAbstractVariable e1, int e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }
        public static ClLinearEquation operator ^(double e1, ClAbstractVariable e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }
        public static ClLinearEquation operator ^(int e1, ClAbstractVariable e2)
        {
            return new ClLinearEquation(e1, e2, ClStrength.Default);
        }


        #endregion
    }
}
