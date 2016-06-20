using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace CrossQuery.Mapper.Internal
{
    internal class MapOperation<TSource, TDest>
        where TDest : class, new()
        where TSource : class
    {
        internal Expression<Func<TDest, object>> DestinationProperyExpression;
        internal Func<TSource, object> GetSourcePropertyValue;

        internal void Map(TSource sourceObj, TDest destObj)
        {
            if (DestinationProperyExpression == null)
                throw new NullReferenceException("DestinationProperyExpression isn't set.");

            if (GetSourcePropertyValue == null)
                throw new NullReferenceException("GetSourcePropertyDelegate isn't set.");

            var expression = DestinationProperyExpression.Body;

            var member = expression as MemberExpression;

            if (member == null)
                if (IsConversion(expression) && expression is UnaryExpression)
                {
                    member = ((UnaryExpression)expression).Operand as MemberExpression;

                    if (member == null)
                        throw new ArgumentException($"Expression '{DestinationProperyExpression.ToString()}' refers to a method, not a property.");
                }
                else
                {
                    throw new ArgumentException($"Expression '{DestinationProperyExpression.ToString()}' refers to a method, not a property.");
                }

            var propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
                throw new ArgumentException($"Expression '{DestinationProperyExpression.ToString()}' refers to a field, not a property.");

            if (!propInfo.CanWrite || propInfo.GetSetMethod() == null)
                throw new ArgumentException($"Expression '{DestinationProperyExpression.ToString()}' is read only property.");

            try
            {
                propInfo.SetValue(destObj, GetSourcePropertyValue(sourceObj), null);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Missing cast for property {propInfo.Name}. {ex.Message}");
            }
        }

        private bool IsConversion(Expression expression)
        {
            return (
                expression.NodeType == ExpressionType.Convert ||
                expression.NodeType == ExpressionType.ConvertChecked
            );
        }
    }
}
