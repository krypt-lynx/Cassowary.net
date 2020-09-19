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
        /// <summary>
        /// implicit variable to expression conversion
        /// </summary>
        /// <param name="expr"></param>
        public static implicit operator ClLinearExpression(ClAbstractVariable expr)
        {
            return new ClLinearExpression(expr);
        }
        /// <summary>
        /// implicit number to expression conversion
        /// </summary>
        /// <param name="expr"></param>
        public static implicit operator ClLinearExpression(int expr)
        {
            return new ClLinearExpression(expr);
        }
        /// <summary>
        /// implicit number to expression conversion
        /// </summary>
        /// <param name="expr"></param>
        public static implicit operator ClLinearExpression(double expr)
        {
            return new ClLinearExpression(expr);
        }
        #endregion

        #region operators
        /// <summary>
        /// returns copy of expression without changing it
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static ClLinearExpression operator +(ClLinearExpression expr)
        {
            return expr.Clone();
        }
        /// <summary>
        /// returns negated copy of expression
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static ClLinearExpression operator -(ClLinearExpression expr)
        {
            return expr.Clone().MultiplyMe(-1);
        }

        /// <summary>
        /// creates linear expression representing summ of two expressions
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearExpression operator +(ClLinearExpression e1, ClLinearExpression e2)
        {
            return e1.Plus(e2);
        }
        /// <summary>
        /// creates linear expression representing substraction of two expressions
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        public static ClLinearExpression operator -(ClLinearExpression e1, ClLinearExpression e2)
        {
            return e1.Minus(e2);
        }
        /// <summary>
        /// creates linear expression representing multiplication of two expressions
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        public static ClLinearExpression operator *(ClLinearExpression e1, ClLinearExpression e2)
        {
            return e1.Times(e2);
        }
        /// <summary>
        /// creates linear expression representing division of two expressions
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        public static ClLinearExpression operator /(ClLinearExpression e1, ClLinearExpression e2)
        {
            return e1.Divide(e2);
        }
        #endregion

        #region comparision (return Constraints) 
        /*
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(ClLinearExpression e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(ClLinearExpression e1, ClLinearExpression e2)
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
        public static ClLinearConstraint operator <=(ClLinearExpression e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(ClLinearExpression e1, ClLinearExpression e2)
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
        public static ClLinearConstraint operator <(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        */
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        /*
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(ClLinearExpression e1, double e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(double e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(ClLinearExpression e1, double e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(double e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        */
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(ClLinearExpression e1, double e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(double e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(ClLinearExpression e1, double e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(double e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        /*
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(ClLinearExpression e1, int e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(int e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >(ClLinearExpression e1, int e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThan, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <(int e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThan, e2, ClStrength.Default);
        }
        */
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(ClLinearExpression e1, int e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(int e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator >=(ClLinearExpression e1, int e2)
        {
            return new ClLinearConstraint(e1, Operator.GreaterThanOrEqualTo, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear inequality constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator <=(int e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, Operator.LessThanOrEqualTo, e2, ClStrength.Default);
        }


        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(ClLinearExpression e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(ClLinearExpression e1, ClAbstractVariable e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(ClLinearExpression e1, double e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(ClLinearExpression e1, int e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(ClAbstractVariable e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(double e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }
        /// <summary>
        /// Creates linear equation constraint with default strenght
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static ClLinearConstraint operator ^(int e1, ClLinearExpression e2)
        {
            return new ClLinearConstraint(e1, e2, ClStrength.Default);
        }

        #endregion
    }
}
