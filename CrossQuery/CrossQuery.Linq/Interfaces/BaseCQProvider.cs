using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CrossQuery.Linq.Collections;
using CrossQuery.Linq.Helpers;

namespace CrossQuery.Linq.Interfaces
{
    public abstract class BaseCQProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);

            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(CQSet<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new CQSet<TElement>(this, expression);
        }

        TEntity IQueryProvider.Execute<TEntity>(Expression expression)
        {
            return (TEntity)this.Execute(expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return this.Execute(expression);
        }

        public abstract object Execute(Expression expression);
    }
}
