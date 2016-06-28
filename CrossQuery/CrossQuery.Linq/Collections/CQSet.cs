using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq.Collections
{
    public class CQSet<T> : IOrderedQueryable<T>
    {
        BaseCQProvider _provider;
        Expression _expression;

        private CQSet()
        {
            if (typeof(ICQObject).IsAssignableFrom(typeof(T)))
                throw new InvalidCastException($"{typeof(T).FullName} don't inmplement interface {typeof(ICQObject).FullName}");
        }

        public CQSet(BaseCQProvider provider) 
            : base()
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            _provider = provider;
            _expression = Expression.Constant(this);
        }

        public CQSet(BaseCQProvider provider, Expression expression)
            : base()
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (expression == null)
                throw new ArgumentNullException("expression");

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
                throw new ArgumentOutOfRangeException("expression");

            _provider = provider;
            _expression = expression;
        }

        Expression IQueryable.Expression
        {
            get
            {
                return _expression;
            }
        }

        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_provider.Execute(_expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_provider.Execute(_expression)).GetEnumerator();
        }
    }
}
