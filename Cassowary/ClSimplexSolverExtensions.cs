﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cassowary
{
    public delegate bool ConstraintExpression1(double arg1);
    public delegate bool ConstraintExpression2(double arg1, double arg2);
    public delegate bool ConstraintExpression3(double arg1, double arg2, double arg3);
    public delegate bool ConstraintExpression4(double arg1, double arg2, double arg3, double arg4);
    public delegate bool ConstraintExpression5(double arg1, double arg2, double arg3, double arg4, double arg5);
    public delegate bool ConstraintExpression6(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6);
    public delegate bool ConstraintExpression7(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7);
    public delegate bool ConstraintExpression8(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8);
    public delegate bool ConstraintExpression9(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8, double arg9);
    public delegate bool ConstraintExpressionA(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8, double arg9, double argA);
    public delegate bool ConstraintExpressionB(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8, double arg9, double argA, double argB);
    public delegate bool ConstraintExpressionC(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8, double arg9, double argA, double argB, double argC);
    public delegate bool ConstraintExpressionD(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8, double arg9, double argA, double argB, double argC, double argD);
    public delegate bool ConstraintExpressionE(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8, double arg9, double argA, double argB, double argC, double argD, double argE);
    public delegate bool ConstraintExpressionF(double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8, double arg9, double argA, double argB, double argC, double argD, double argE, double argF);

    public static class ClSimplexSolverExtensions
    {
        private static readonly ClStrength _defaultStrength = ClStrength.Required;

        #region add expression constraint
        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpression1> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, Expression<ConstraintExpression1> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpression2> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, Expression<ConstraintExpression2> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpression3> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, Expression<ConstraintExpression3> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpression4> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, Expression<ConstraintExpression4> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpression5> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e, Expression<ConstraintExpression5> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpression6> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
             ClAbstractVariable f, Expression<ConstraintExpression6> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpression7> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
            ClAbstractVariable f, ClAbstractVariable g, Expression<ConstraintExpression7> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f, g);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpression8> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
            ClAbstractVariable f, ClAbstractVariable g, ClAbstractVariable h, Expression<ConstraintExpression8> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f, g, h);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpression9> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
            ClAbstractVariable f, ClAbstractVariable g, ClAbstractVariable h, ClAbstractVariable i, Expression<ConstraintExpression9> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f, g, h, i);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpressionA> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
            ClAbstractVariable f, ClAbstractVariable g, ClAbstractVariable h, ClAbstractVariable i, ClAbstractVariable j, Expression<ConstraintExpressionA> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f, g, h, i, j);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpressionB> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
            ClAbstractVariable f, ClAbstractVariable g, ClAbstractVariable h, ClAbstractVariable i, ClAbstractVariable j, ClAbstractVariable k, Expression<ConstraintExpressionB> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f, g, h, i, j, k);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpressionC> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
            ClAbstractVariable f, ClAbstractVariable g, ClAbstractVariable h, ClAbstractVariable i, ClAbstractVariable j, ClAbstractVariable k, ClAbstractVariable l, Expression<ConstraintExpressionC> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f, g, h, i, j, k, l);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpressionD> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
            ClAbstractVariable f, ClAbstractVariable g, ClAbstractVariable h, ClAbstractVariable i, ClAbstractVariable j, ClAbstractVariable k, ClAbstractVariable l, ClAbstractVariable m, Expression<ConstraintExpressionD> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f, g, h, i, j, k, l, m);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpressionE> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
            ClAbstractVariable f, ClAbstractVariable g, ClAbstractVariable h, ClAbstractVariable i, ClAbstractVariable j, ClAbstractVariable k, ClAbstractVariable l, ClAbstractVariable m, ClAbstractVariable n, Expression<ConstraintExpressionE> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f, g, h, i, j, k, l, m, n);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, Expression<ConstraintExpressionF> constraint, ClStrength strength = null)
        {
            return AddConstraint(solver, constraint.Parameters, constraint.Body, strength);
        }

        public static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, ClAbstractVariable a, ClAbstractVariable b, ClAbstractVariable c, ClAbstractVariable d, ClAbstractVariable e,
            ClAbstractVariable f, ClAbstractVariable g, ClAbstractVariable h, ClAbstractVariable i, ClAbstractVariable j, ClAbstractVariable k, ClAbstractVariable l, ClAbstractVariable m, ClAbstractVariable n, ClAbstractVariable o, Expression<ConstraintExpressionF> constraint, ClStrength strength = null)
        {
            Dictionary<string, ClAbstractVariable> variables = ConstructVariables(constraint.Parameters, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o);
            return AddConstraint(solver, variables, constraint.Body, strength);
        }

        private static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, IEnumerable<ParameterExpression> parameters, Expression body, ClStrength strength)
        {
            Dictionary<string, ClAbstractVariable> variables = parameters.Select(a => solver.GetVariable(a.Name) ?? new ClVariable(a.Name)).ToDictionary(a => a.Name);
            return AddConstraint(solver, variables, body, strength);
        }

        private static ClSimplexSolver AddConstraint(this ClSimplexSolver solver, IDictionary<string, ClAbstractVariable> variables, Expression body, ClStrength strength)
        {
            var constraints = FromExpression(variables, body, strength ?? _defaultStrength);
            foreach (var c in constraints)
                solver.AddConstraint(c);

            return solver;
        }

        private static Dictionary<string, ClAbstractVariable> ConstructVariables(ICollection<ParameterExpression> parameters, params ClAbstractVariable[] variables)
        {
            if (variables.Length != parameters.Count)
                throw new ArgumentException(string.Format("Expected {0} parameters, found {1}", parameters.Count, variables.Length));

            return parameters.Select((p, i) => new {name = p.Name, variable = variables[i]})
                .ToDictionary(a => a.name, a => a.variable);
        }
        #endregion

        #region expression conversion
        private static IEnumerable<ClConstraint> FromExpression(IDictionary<string, ClAbstractVariable> variables, Expression expression, ClStrength strength)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                {
                    foreach (var c in FromExpression(variables, ((BinaryExpression) expression).Left, strength))
                        yield return c;
                    foreach (var c in FromExpression(variables, ((BinaryExpression) expression).Right, strength))
                        yield return c;
                    break;
                }
                case ExpressionType.Equal:
                {
                    yield return CreateEquality(variables, (BinaryExpression) expression, strength);
                    break;
                }
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThan:
                {
                    yield return CreateLinearInequality(variables, (BinaryExpression) expression, strength);
                    break;
                }
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
                case ExpressionType.Parameter:
                    return new ClLinearExpression(variables[((ParameterExpression)a).Name]);
                case ExpressionType.Constant:
                case ExpressionType.MemberAccess:
                case ExpressionType.Convert:
                    return new ClLinearExpression(GetValue(a));
                default:
                    throw new ArgumentException(string.Format("Invalid node type {0}", a.NodeType), "a");
            }
        }

        private static double GetValue(Expression a)
        {
            switch (a.NodeType)
            {
                case ExpressionType.Constant:
                    return Convert.ToDouble(((ConstantExpression)a).Value);
                case ExpressionType.MemberAccess:
                    var memberAccess = (MemberExpression)a;
                    var fieldInfo = memberAccess.Member as FieldInfo;
                    if (fieldInfo != null)
                        return Convert.ToDouble(fieldInfo.GetValue(Expression.Lambda<Func<object>>(memberAccess.Expression).Compile().Invoke()));
                    
                        var info = memberAccess.Member as PropertyInfo;
                    if (info != null)
                        return Convert.ToDouble(info.GetValue(Expression.Lambda<Func<object>>(memberAccess.Expression).Compile().Invoke(), null));
                        
                    throw new ArgumentException(string.Format("Invalid node type MemberAccess, {0}", memberAccess.Member.GetType()));
                case ExpressionType.Convert:
                    var e = (UnaryExpression)a;
                    var v = GetValue(e.Operand);
                    return v;
                case ExpressionType.Add:
                    {
                        var b = (BinaryExpression)a;
                        return GetValue(b.Left) + GetValue(b.Right);
                    }
                case ExpressionType.Subtract:
                    {
                        var b = (BinaryExpression)a;
                        return GetValue(b.Left) - GetValue(b.Right);
                    }
                case ExpressionType.Multiply:
                    {
                        var b = (BinaryExpression)a;
                        return GetValue(b.Left) * GetValue(b.Right);
                    }
                case ExpressionType.Divide:
                    {
                        var b = (BinaryExpression)a;
                        return GetValue(b.Left) / GetValue(b.Right);
                    }
                case ExpressionType.Negate:
                {
                    var u = (UnaryExpression)a;
                    return -GetValue(u.Operand);
                }
                default:
                    throw new ArgumentException(string.Format("Invalid node type {0}", a.NodeType), "a");
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
        #endregion
    }
}
