using System;
using System.Linq;
using System.Linq.Dynamic;
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

        private Type _entityType;
        private StringBuilder _lambdaExpression = new StringBuilder();
        private string _methodName;

        internal QueryBuilder(IDataAdapter dataAdapter, Expression sourceExpression)
        {
            _dataAdapter = dataAdapter;
            _expression = sourceExpression;
        }

        internal Query Build()
        {
            this.Visit(_expression);

            var parameterExpression = Expression.Parameter(_entityType, "x");
            var lambda = System.Linq.Dynamic.DynamicExpression.ParseLambda(new[] { parameterExpression }, null, _lambdaExpression.ToString(), null);

            return new Query
            {
                Type = _methodName,
                EntityType = _entityType,
                LambdaExpression = lambda
            };
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

            _methodName = methodCallExpression.Method.Name;
            var lambda = (LambdaExpression)StripQuotes(methodCallExpression.Arguments[1]);

            this.Visit(lambda.Body);

            return methodCallExpression;
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            throw new NotImplementedException();
        }

        protected override Expression VisitBinary(BinaryExpression binaryExpression)
        {
            this.Visit(binaryExpression.Left);

            switch (binaryExpression.NodeType)
            {
                case ExpressionType.And:
                    _lambdaExpression.Append("&&");
                    break;
                case ExpressionType.Or:
                    _lambdaExpression.Append("||");
                    break;
                case ExpressionType.Equal:
                    _lambdaExpression.Append("==");
                    break;
                case ExpressionType.NotEqual:
                    _lambdaExpression.Append("<>");
                    break;
                case ExpressionType.LessThan:
                    _lambdaExpression.Append("<");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _lambdaExpression.Append("<=");
                    break;
                case ExpressionType.GreaterThan:
                    _lambdaExpression.Append(">");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _lambdaExpression.Append(">=");
                    break;
                default:
                    throw new NotSupportedException($"The binary operator '{binaryExpression.NodeType}' is not supported");
            }

            this.Visit(binaryExpression.Right);

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

                _entityType = ((AdapterAttribute)attribute).SourceClass;
            }

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

                _lambdaExpression.Append($" x.{memberExpression.Member.Name}.ToString() ");
            }
            else
            {
                if (memberExpression.Member is PropertyInfo)
                {
                    var memberExpression2 = (MemberExpression)memberExpression.Expression;
                    var constantExpression = (ConstantExpression)memberExpression2.Expression;
                    var fieldInfo = ((FieldInfo)memberExpression2.Member).GetValue(constantExpression.Value);

                    _lambdaExpression.Append($" \"{((PropertyInfo)memberExpression.Member).GetValue(fieldInfo, null)}\" ");
                }
            }

            return memberExpression;
        }
    }
}
