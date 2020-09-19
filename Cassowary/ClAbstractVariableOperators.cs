using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cassowary_moddiff.Cl;

namespace Cassowary_moddiff
{
    public abstract partial class ClAbstractVariable
    {
        // say hello to combinatoric explosion

        /// <summary>
        /// creates linear expression representing summ of to variables
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator +(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }

        /// <summary>
        /// creates linear expression representing summ of to variable and constant number
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator +(ClAbstractVariable e1, double e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }
        /// <summary>
        /// creates linear expression representing summ of to variable and constant number
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator +(double e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }
        /// <summary>
        /// creates linear expression representing summ of to variable and constant number
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator +(ClAbstractVariable e1, int e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }
        /// <summary>
        /// creates linear expression representing summ of to variable and constant number
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator +(int e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2));
        }

        /// <summary>
        /// creates linear expression representing sunstracion 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator -(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }
        /// <summary>
        /// creates linear expression representing sunstracion 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator -(ClAbstractVariable e1, double e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }
        /// <summary>
        /// creates linear expression representing sunstracion 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator -(double e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }
        /// <summary>
        /// creates linear expression representing sunstracion 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator -(ClAbstractVariable e1, int e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }
        /// <summary>
        /// creates linear expression representing sunstracion 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator -(int e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e1).AddExpression(new ClLinearExpression(e2), -1.0);
        }


        // no variable*variable: exlressions are linear and can have only x^1 components
        /// <summary>
        /// creates linear expression representing multiplication of variable by number 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator *(ClAbstractVariable e1, double e2)
        {
            return new ClLinearExpression(e1).MultiplyMe(e2);
        }
        /// <summary>
        /// creates linear expression representing multiplication of variable by number 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator *(double e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e2).MultiplyMe(e1);
        }
        /// <summary>
        /// creates linear expression representing multiplication of variable by number 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator *(ClAbstractVariable e1, int e2)
        {
            return new ClLinearExpression(e1).MultiplyMe(e2);
        }
        /// <summary>
        /// creates linear expression representing multiplication of variable by number 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator *(int e1, ClAbstractVariable e2)
        {
            return new ClLinearExpression(e2).MultiplyMe(e1);
        }


        // no x/variable and variable/variable: exlressions are linear and can have only x^1 components
        /// <summary>
        /// creates linear expression representing division of variable by number 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator /(ClAbstractVariable e1, int e2)
        {
            return new ClLinearExpression(e1).DivideMe(e2);
        }
        /// <summary>
        /// creates linear expression representing division of variable by number 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>linear expression</returns>
        public static ClLinearExpression operator /(ClAbstractVariable e1, double e2)
        {
            return new ClLinearExpression(e1).DivideMe(e2);
        }


        #region comparision (return Constraints) 
        /*
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        */
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        /*
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(ClAbstractVariable e1, double e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(double e1, ClAbstractVariable e2)
        {
            return e2 < e1;
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(ClAbstractVariable e1, double e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(double e1, ClAbstractVariable e2)
        {
            return e2 > e1;
        }
        */
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(ClAbstractVariable e1, double e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(double e1, ClAbstractVariable e2)
        {
            return e2 <= e1;
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(ClAbstractVariable e1, double e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(double e1, ClAbstractVariable e2)
        {
            return e2 >= e1;
        }
        /*
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(ClAbstractVariable e1, int e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(int e1, ClAbstractVariable e2)
        {
            return e2 < e1;
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(ClAbstractVariable e1, int e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(int e1, ClAbstractVariable e2)
        {
            return e2 > e1;
        }
        */
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(ClAbstractVariable e1, int e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(int e1, ClAbstractVariable e2)
        {
            return e2 <= e1;
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(ClAbstractVariable e1, int e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(int e1, ClAbstractVariable e2)
        {
            return e2 >= e1;
        }

        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(ClAbstractVariable e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(ClAbstractVariable e1, double e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(ClAbstractVariable e1, int e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(double e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(int e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }

        #endregion
    }
}
