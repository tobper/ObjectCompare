using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectCompare.Expressions
{
    public static class ExpressionExtensions
    {
        private static readonly Expression Null = Expression.Constant(null);

        public static Expression AsNew(this Type type)
        {
            return Expression.New(type);
        }

        public static Expression AsNew(this ParameterExpression variable)
        {
            return Expression.
                New(variable.Type).
                AsAssign(variable);
        }

        public static Expression AsNew(this ParameterExpression expression, ConstructorInfo constructorInfo, params Expression[] arguments)
        {
            return Expression.
                New(constructorInfo, arguments).
                AsAssign(expression);
        }

        public static Expression<TDelegate> AsLambda<TDelegate>(this Expression body, params ParameterExpression[] parameters)
        {
            return Expression.Lambda<TDelegate>(body, parameters);
        }

        public static BlockExpression AsBlock(this IEnumerable<Expression> expressions)
        {
            return Expression.Block(expressions);
        }

        public static BlockExpression AsBlock(this IEnumerable<Expression> expressions, IEnumerable<ParameterExpression> variables)
        {
            return Expression.Block(variables, expressions);
        }

        public static BlockExpression AsBlock(this IEnumerable<Expression> expressions, params ParameterExpression[] variables)
        {
            return Expression.Block(variables, expressions);
        }

        public static LoopExpression AsLoop(this Expression expression)
        {
            return Expression.Loop(expression);
        }

        public static LoopExpression AsLoop(this Expression expression, LabelTarget breakTarget)
        {
            return Expression.Loop(expression, breakTarget);
        }

        public static ParameterExpression AsParameter(this Type type)
        {
            return Expression.Parameter(type);
        }

        public static ParameterExpression AsParameter(this Type type, string name)
        {
            return Expression.Parameter(type, name);
        }

        public static GotoExpression AsReturn(this LabelTarget target, object value)
        {
            return AsReturn(target, Expression.Constant(value));
        }

        public static GotoExpression AsReturn(this LabelTarget target, Expression value)
        {
            return Expression.Return(target, value);
        }

        public static GotoExpression AsBreak(this LabelTarget target)
        {
            return Expression.Break(target);
        }

        public static LabelExpression AsExpression(this LabelTarget target, object value)
        {
            return AsExpression(target, Expression.Constant(value));
        }

        public static LabelExpression AsExpression(this LabelTarget target, Expression value)
        {
            return Expression.Label(target, value);
        }

        public static MethodCallExpression AsCall(this MethodInfo method)
        {
            return Expression.Call(method);
        }

        public static MethodCallExpression AsCall(this MethodInfo method, params Expression[] arguments)
        {
            return Expression.Call(method, arguments);
        }

        public static MethodCallExpression AsInstanceCall(this MethodInfo method, Expression instance)
        {
            return Expression.Call(instance, method);
        }

        public static MethodCallExpression AsInstanceCall(this MethodInfo method, Expression instance, params Expression[] arguments)
        {
            return Expression.Call(instance, method, arguments);
        }

        public static MethodCallExpression InstanceCall(this ParameterExpression instance, MethodInfo method)
        {
            return Expression.Call(instance, method);
        }

        public static MethodCallExpression InstanceCall(this ParameterExpression instance, MethodInfo method, params Expression[] arguments)
        {
            return Expression.Call(instance, method, arguments);
        }

        public static BinaryExpression Is(this Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }

        public static BinaryExpression IsNot(this Expression left, Expression right)
        {
            return Expression.NotEqual(left, right);
        }

        public static BinaryExpression IsNull(this Expression value)
        {
            return Is(value, Null);
        }

        public static BinaryExpression IsNotNull(this Expression value)
        {
            return IsNot(value, Null);
        }

        public static UnaryExpression IsFalse(this Expression expression)
        {
            return Expression.IsFalse(expression);
        }

        public static UnaryExpression IsTrue(this Expression expression)
        {
            return Expression.IsTrue(expression); 
        }

        public static UnaryExpression Not(this Expression expression)
        {
            return Expression.Not(expression);
        }

        public static ParameterExpression AsVariable(this Type type)
        {
            return Expression.Variable(type);
        }

        public static ParameterExpression AsVariable(this Type type, string name)
        {
            return Expression.Variable(type, name);
        }

        public static BinaryExpression AsAssign(this Expression value, ParameterExpression target)
        {
            return Expression.Assign(target, value);
        }

        public static MemberExpression Value(this FieldInfo field, Expression value)
        {
            return Expression.Field(value, field);
        }
    }
}