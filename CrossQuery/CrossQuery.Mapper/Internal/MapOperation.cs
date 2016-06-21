using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
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

            var destinationPropertyInfo = GetPropertyInfo(DestinationProperyExpression.Body);
            var sourcePropertyValue = GetSourcePropertyValue(sourceObj);

            try
            {
                if (IsCollection(destinationPropertyInfo.PropertyType)
                && IsCollection(sourcePropertyValue.GetType()))
                {
                    sourcePropertyValue = typeof(Mapper)
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .First(m => m.Name == "Map" && IsCollection(m.ReturnType))
                        .MakeGenericMethod(new Type[]
                        {
                            sourcePropertyValue.GetType().GetGenericArguments().First(),
                            destinationPropertyInfo.PropertyType.GetGenericArguments().First()
                        })
                        .Invoke(null, new[]
                        {
                            ((IEnumerable)sourcePropertyValue).AsQueryable()
                        });
                }
                else if (IsReferenceType(destinationPropertyInfo.PropertyType)
                && IsReferenceType(sourcePropertyValue.GetType()))
                {

                    sourcePropertyValue = typeof(Mapper)
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .First(m => m.Name == "Map" && !IsCollection(m.ReturnType))
                        .MakeGenericMethod(new Type[]
                        {
                            sourcePropertyValue.GetType(),
                            destinationPropertyInfo.PropertyType
                        })
                        .Invoke(null, new[]
                        {
                            sourcePropertyValue
                        });
                }
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }

            try
            {
                destinationPropertyInfo.SetValue(destObj, sourcePropertyValue, null);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Missing cast for property {destinationPropertyInfo.Name} to {sourcePropertyValue.GetType().Name}. {ex.Message}");
            }
        }

        private PropertyInfo GetPropertyInfo(Expression expression)
        {
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

            var propertyInfo = member.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException($"Expression '{DestinationProperyExpression.ToString()}' refers to a field, not a property.");

            if (!propertyInfo.CanWrite || propertyInfo.GetSetMethod() == null)
                throw new ArgumentException($"Expression '{DestinationProperyExpression.ToString()}' is read only property.");

            return propertyInfo;
        }

        private bool IsConversion(Expression expression)
        {
            return (
                expression.NodeType == ExpressionType.Convert ||
                expression.NodeType == ExpressionType.ConvertChecked
            );
        }

        private bool IsReferenceType(Type type)
        {
            if (type.IsClass && type != typeof(String))
                return true;

            return false;        
        }

        private bool IsCollection(Type type)
        {
            if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type))
                return true;

            return false;
        }
    }
}
