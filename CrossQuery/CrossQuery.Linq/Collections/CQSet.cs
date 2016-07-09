using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq.Collections
{
    public class CQSet<T> : IOrderedQueryable<T>
    {
        BaseCQProvider _provider;
        Expression _expression;

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
            // ToDo Needs refactoring
            var resultCollection = _provider.Execute(_expression);
            var dynamicClassType = resultCollection.GetType().GetGenericArguments()[0];

            if (typeof(DynamicClass).IsAssignableFrom(dynamicClassType))
            {
                var properties = dynamicClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var ananimusCollection = new List<T>();

                foreach (var element in (IQueryable)resultCollection)
                    ananimusCollection.Add((T)Activator.CreateInstance(typeof(T), properties
                        .Select(p => p.GetValue(element)).ToArray()));

                resultCollection = ananimusCollection;
            }

            return ((IEnumerable<T>)resultCollection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_provider.Execute(_expression)).GetEnumerator();
        }
    }
}
