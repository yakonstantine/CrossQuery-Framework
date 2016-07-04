using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using CrossQuery.Interfaces;
using CrossQuery.Linq.Attributes;

namespace CrossQuery.Linq
{
    internal class QueryBuilder : ExpressionVisitor
    {
        private IDataAdapter _dataAdapter;
        private Expression _expression;

        private Query _query = new Query();

        internal QueryBuilder(IDataAdapter dataAdapter, Expression sourceExpression)
        {
            _dataAdapter = dataAdapter;
            _expression = sourceExpression;
        }

        internal Query Build()
        {
            this.Visit(_expression);

            return _query;
        }

        private static Expression StripQuotes(Expression expression)
        {
            while (expression.NodeType == ExpressionType.Quote)
                expression = ((UnaryExpression)expression).Operand;

            return expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            this.Visit(methodCallExpression.Arguments[0]);

            if (_query.MethodName == methodCallExpression.Method.Name)
                _query.AppendBinaryOperator(ExpressionType.AndAlso);

            _query.MethodName = methodCallExpression.Method.Name;

            if (methodCallExpression.Arguments.Count > 1)
            {                
                var lambda = (LambdaExpression)StripQuotes(methodCallExpression.Arguments[1]);

                this.Visit(lambda.Body);
            }

            return methodCallExpression;
        }

        protected override Expression VisitNew(NewExpression newExpression)
        {
            _query.LambdaExpression.Append("new (");

            for (var i = 0; i < newExpression.Arguments.Count; i++)
            {
                if (i != 0)
                    _query.LambdaExpression.Append(", ");

                this.Visit(newExpression.Arguments[i]);
            }

            _query.LambdaExpression.Append(")");

            return newExpression;
        }

        protected override Expression VisitUnary(UnaryExpression unaryExpression)
        {
            switch (unaryExpression.NodeType)
            {
                case ExpressionType.Not:
                case ExpressionType.NotEqual:
                    _query.LambdaExpression.Append("!");
                    break;
            }

            this.Visit(unaryExpression.Operand);

            return unaryExpression;
        }

        protected override Expression VisitBinary(BinaryExpression binaryExpression)
        {
            _query.LambdaExpression.Append("(");

            this.Visit(binaryExpression.Left);

            _query.AppendBinaryOperator(binaryExpression.NodeType);

            this.Visit(binaryExpression.Right);

            _query.LambdaExpression.Append(")");

            return binaryExpression;
        }

        protected override Expression VisitConstant(ConstantExpression constantExpression)
        {
            var q = constantExpression.Value as IQueryable;

            if (q != null)
            {
                var attribute = Attribute.GetCustomAttribute(q.ElementType, typeof(AdapterAttribute));

                if (attribute == null)
                    throw new InvalidOperationException($"Attribute {typeof(AdapterAttribute)} is not added to class.");

                var adapterAttribute = (AdapterAttribute)attribute;

                if (_dataAdapter.Name != adapterAttribute.AdapterName)
                    return constantExpression;

                _query.EntityType = ((AdapterAttribute)attribute).SourceClass;
            }
            else if (constantExpression.NodeType == ExpressionType.Constant)
                _query.AddParameter(constantExpression.Value);

            return constantExpression;
        }

        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            if (memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.Parameter)
            {
                var attribute = memberExpression.Member.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(AssotiationAttribute));

                if (attribute != null)
                {
                    throw new NotImplementedException();
                }

                _query.LambdaExpression.Append($"x.{memberExpression.Member.Name}");
            }
            else
            {
                if (memberExpression.Member is PropertyInfo)
                {
                    var memberExpression2 = (MemberExpression)memberExpression.Expression;
                    var constantExpression = (ConstantExpression)memberExpression2.Expression;
                    var fieldInfo = ((FieldInfo)memberExpression2.Member).GetValue(constantExpression.Value);

                    _query.AddParameter(((PropertyInfo)memberExpression.Member).GetValue(fieldInfo, null));
                }
            }

            return memberExpression;
        }
    }
}
