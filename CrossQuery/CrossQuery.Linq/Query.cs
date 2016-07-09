using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace CrossQuery.Linq
{
    internal class Query
    {
        internal string MethodName { get; set; }
        internal Type EntityType { get; set; }
        internal StringBuilder LambdaExpression { get; set; } = new StringBuilder();
        internal List<object> Parameters { get; set; } = new List<object>();

        internal void AppendBinaryOperator(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:                
                    this.LambdaExpression.Append(" && ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:                
                    this.LambdaExpression.Append(" || ");
                    break;
                case ExpressionType.Equal:
                    this.LambdaExpression.Append(" == ");
                    break;
                case ExpressionType.NotEqual:
                    this.LambdaExpression.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    this.LambdaExpression.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    this.LambdaExpression.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    this.LambdaExpression.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    this.LambdaExpression.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException($"The binary operator '{type}' is not supported");
            }
        }

        internal void AddParameter(object parameter)
        {
            this.LambdaExpression.Append($" @{this.Parameters.Count()} ");
            this.Parameters.Add(parameter);
        }

        internal object Execute(IQueryable collection)
        {
            var method = typeof(DynamicQueryable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == this.MethodName && m.ReturnType == typeof(IQueryable));

            if (method != null)
            {
                return method
                .Invoke(collection,
                new object[]
                {
                    collection,
                    this.LambdaExpression.ToString(),
                    this.Parameters.ToArray()
                });
            }

            method = typeof(Queryable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == this.MethodName && m.GetParameters().Count() == 2);

            if (method != null)
            {
                return method
                    .MakeGenericMethod(this.EntityType)
                    .Invoke(collection,
                    new object[]
                    {
                        collection,
                        this.CreateLambdaExpression()
                    });
            }

            throw new NotImplementedException($"Method name - {this.MethodName}");
        }

        internal Expression CreateLambdaExpression()
        {
            return System.Linq.Dynamic.DynamicExpression.ParseLambda(
                 new[] { Expression.Parameter(this.EntityType) },
                 null,
                 this.LambdaExpression.ToString(),
                 this.Parameters.ToArray()
                 );
        }
    }
}
