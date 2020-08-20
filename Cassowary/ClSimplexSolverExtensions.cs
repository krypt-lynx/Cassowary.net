using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// this file is a fine example of stackoverflow programming

namespace Cassowary
{
    /// <summary>
    /// Constraints builder extension
    /// </summary>
    public static class ConstraintsBuilder
    {

        /// <summary>
        /// Converts linq expression to set of constraints and adds them to the solver
        /// </summary>
        /// <param name="solver">solver</param>
        /// <param name="expression">linq expressin representing constraint</param>
        /// <param name="strength">strength of created constraints. if parameter is null ClStrength.Default is used</param>
        /// <returns>the solver</returns>
        /// <remarks>
        /// This method traverses given linq exression in search of supported operand types\n
        /// Generic code support is pretty limited outside of simple method calls. If you got ArgumentException during call of the method try to simlpfy passed code.\n
        /// And don't forget a report an issue with exact code you put into it\n
        /// Also, Generic code in this method is discoraged: it will be compiled in runtime on user's mathine instead of compilation time on yours.
        /// </remarks>
        /// <exception cref="System.ArgumentException">Unsupported expression tree</exception>
        /// <exception cref="Cassowary.CassowaryNonlinearExpressionException">resulted expression in not linear can cannot be solved</exception>
        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, Expression<Func<bool>> expression, ClStrength strength = null)
        {
            solver.AddConstraints(CreateConstraints(expression, strength));
            return solver;
        }

        public static IEnumerable<ClConstraint> CreateConstraints(Expression<Func<bool>> expression, ClStrength strength = null)
        {
            if (strength == null)
            {
                strength = ClStrength.Default;
            }

            return FromExpression(new Dictionary<string, ClAbstractVariable>(), expression.Body, strength);
        }

        public static ClSimplexSolver AddConstraints(this ClSimplexSolver solver, IEnumerable<ClConstraint> constraints)
        {
            foreach (var constraint in constraints)
            {
                solver.AddConstraint(constraint);
            }
            return solver;
        }



        #region expression conversion
        private static IEnumerable<ClConstraint> FromExpression(IDictionary<string, ClAbstractVariable> variables, Expression expression, ClStrength strength)
        {
            switch (expression.NodeType)
            { 
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    {
                        foreach (var c in FromExpression(variables, ((BinaryExpression)expression).Left, strength))
                            yield return c;
                        foreach (var c in FromExpression(variables, ((BinaryExpression)expression).Right, strength))
                            yield return c;
                        break;
                    }
                case ExpressionType.Equal:
                    {
                        yield return CreateEquality(variables, (BinaryExpression)expression, strength);
                        break;
                    }
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThan:
                    {
                        yield return CreateLinearInequality(variables, (BinaryExpression)expression, strength);
                        break;
                    }
                default:
                    throw new ArgumentException(string.Format("Invalid node type {0}", expression.NodeType), "expression");
            }
        }

        private static ClLinearExpression CreateLinearExpression(IDictionary<string, ClAbstractVariable> variables, Expression a)
        {
            switch (a.NodeType)
            {
                case ExpressionType.Add:
                    {
                        var b = (BinaryExpression)a;
                        return Cl.Plus(CreateLinearExpression(variables, b.Left), CreateLinearExpression(variables, b.Right));
                    }
                case ExpressionType.Subtract:
                    {
                        var b = (BinaryExpression)a;
                        return Cl.Minus(CreateLinearExpression(variables, b.Left), CreateLinearExpression(variables, b.Right));
                    }
                case ExpressionType.Multiply:
                    {
                        var b = (BinaryExpression)a;
                        return Cl.Times(CreateLinearExpression(variables, b.Left), CreateLinearExpression(variables, b.Right));
                    }
                case ExpressionType.Divide:
                    {
                        var b = (BinaryExpression)a;
                        return Cl.Divide(CreateLinearExpression(variables, b.Left), CreateLinearExpression(variables, b.Right));
                    }
                case ExpressionType.Negate:
                    {
                        var u = (UnaryExpression)a;
                        return Cl.Times(CreateLinearExpression(variables, u.Operand), -1);
                    }
                case ExpressionType.Parameter:
                    return new ClLinearExpression(variables[((ParameterExpression)a).Name]);
                case ExpressionType.Constant:
                case ExpressionType.MemberAccess:
                case ExpressionType.Convert:
                case ExpressionType.New:
                case ExpressionType.Invoke:
                case ExpressionType.ArrayIndex:
                    return GetValue(variables, a);
                default:
                    throw new ArgumentException(string.Format("Invalid node type {0} on node {1}", a.NodeType, a.ToString()), "a");
            }
        }

        private static ClLinearExpression GetValue(IDictionary<string, ClAbstractVariable> variables, Expression a)
        {
            switch (a.NodeType)
            {
                case ExpressionType.Constant:
                    return toLinearExpression(((ConstantExpression)a).Value);
                case ExpressionType.MemberAccess:
                    var memberAccess = (MemberExpression)a;
                    var fieldInfo = memberAccess.Member as FieldInfo;
                    if (fieldInfo != null)
                    {
                        var visitor = new BuilderExpressionVisitor();
                        Expression a2 = visitor.Visit(a);
                        return CreateLinearExpression(variables, a2);
                    }

                    var info = memberAccess.Member as PropertyInfo;
                    if (info != null) 
                        return toLinearExpression(info.GetValue(Expression.Lambda<Func<object>>(memberAccess.Expression).Compile().Invoke())); // TODO: is it correct?

                    throw new ArgumentException(string.Format("Invalid node type MemberAccess, {0}", memberAccess.Member.GetType()));
                case ExpressionType.Convert:
                    var e = (UnaryExpression)a;
                    var v = GetValue(variables, e.Operand);
                    return v;
                case ExpressionType.Add:
                    {
                        var b = (BinaryExpression)a;
                        return Cl.Plus(GetValue(variables, b.Left), GetValue(variables, b.Right));
                    }
                case ExpressionType.Subtract:
                    {
                        var b = (BinaryExpression)a;
                        return Cl.Minus(GetValue(variables, b.Left), GetValue(variables, b.Right));
                    }
                case ExpressionType.Multiply:
                    {
                        var b = (BinaryExpression)a;
                        return Cl.Times(GetValue(variables, b.Left), GetValue(variables, b.Right));
                    }
                case ExpressionType.Divide:
                    {
                        var b = (BinaryExpression)a;
                        return Cl.Divide(GetValue(variables, b.Left), GetValue(variables, b.Right));
                    }
                case ExpressionType.Negate:
                    {
                        var u = (UnaryExpression)a;
                        return Cl.Times(GetValue(variables, u.Operand), -1);
                    }
                case ExpressionType.New:               
                case ExpressionType.Invoke:
                case ExpressionType.ArrayIndex:
                    {
                        var r = Expression.Lambda(a).Compile().DynamicInvoke(new object[] { });
                        return toLinearExpression(r);
                    }
                default:
                    throw new ArgumentException(string.Format("Invalid node type {0} of node {1}", a.NodeType, a), "a");
            }
        }

        private static ClConstraint CreateEquality(IDictionary<string, ClAbstractVariable> variables, BinaryExpression expression, ClStrength strength)
        {
            return new ClLinearEquation(CreateLinearExpression(variables, expression.Left), CreateLinearExpression(variables, expression.Right), strength);
        }

        private static ClLinearInequality CreateLinearInequality(IDictionary<string, ClAbstractVariable> variables, BinaryExpression expression, ClStrength strength)
        {
            Cl.Operator op;
            switch (expression.NodeType)
            {
                case ExpressionType.GreaterThan:
                    op = Cl.Operator.GreaterThan;
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    op = Cl.Operator.GreaterThanOrEqualTo;
                    break;
                case ExpressionType.LessThan:
                    op = Cl.Operator.LessThan;
                    break;
                case ExpressionType.LessThanOrEqual:
                    op = Cl.Operator.LessThanOrEqualTo;
                    break;
                default:
                    throw new NotSupportedException(string.Format("Unsupported linear inequality operator \"{0}\"", expression.NodeType));
            }

            return new ClLinearInequality(CreateLinearExpression(variables, expression.Left), op, CreateLinearExpression(variables, expression.Right), strength);
        }
        

        private static ClLinearExpression toLinearExpression(object obj)
        {
            if (obj is ClLinearExpression)
            {
                return (ClLinearExpression)obj;
            }
            if (obj is ClAbstractVariable)
            {
                return new ClLinearExpression(obj as ClAbstractVariable);
            }

            return new ClLinearExpression(Convert.ToDouble(obj));
        }
        #endregion

        public class BuilderExpressionVisitor : ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression node)
            {
                switch (node.Expression.NodeType)
                {
                    case ExpressionType.Constant:
                    case ExpressionType.MemberAccess:
                        {
                            var cleanNode = GetMemberConstant(node);

                            //Test
                            //Assert.AreEqual(1L, cleanNode.Value);

                            return cleanNode;
                        }
                    default:
                        {
                            return base.VisitMember(node);
                        }
                }
            }


            private static ConstantExpression GetMemberConstant(MemberExpression node)
            {
                object value;

                if (node.Member.MemberType == MemberTypes.Field)
                {
                    value = GetFieldValue(node);
                }
                else if (node.Member.MemberType == MemberTypes.Property)
                {
                    value = GetPropertyValue(node);
                }
                else
                {
                    throw new NotSupportedException();
                }

                return Expression.Constant(value, node.Type);
            }
            private static object GetFieldValue(MemberExpression node)
            {
                return Expression.Lambda(node).Compile().DynamicInvoke(new object[] { });
            }

            private static object GetPropertyValue(MemberExpression node)
            {
                //var propertyInfo = (PropertyInfo)node.Member;

                //var instance = (node.Expression == null) ? null : TryEvaluate(node.Expression).Value;

                //return propertyInfo.GetValue(instance, null);

                return Expression.Lambda(node).Compile().DynamicInvoke(new object[] { }); // not sure is it right? Never seen it called

            }

            private static ConstantExpression TryEvaluate(Expression expression)
            {
                if (expression.NodeType == ExpressionType.Constant)
                {
                    return (ConstantExpression)expression;
                }
                throw new NotSupportedException(string.Format("Invalid node type {0} of node {1}", expression.NodeType, expression));
            }

        }    
    }
}
