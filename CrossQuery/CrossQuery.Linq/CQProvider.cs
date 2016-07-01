using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using CrossQuery.Interfaces;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq
{
    public class CQProvider : BaseCQProvider
    {
        private IDataAdapter[] _dataAdapters;

        public CQProvider(params IDataAdapter[] dataAdapters)
        {
            _dataAdapters = dataAdapters;
        }

        public override object Execute(Expression expression)
        {
            foreach(var dataAdapter in _dataAdapters)
            {
                var ex = new QueryBuilder(dataAdapter, expression).Build();
            }

            throw new NotImplementedException();
        }
    }
}
