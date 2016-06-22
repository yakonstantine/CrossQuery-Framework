using System;
using System.Linq;
using System.Linq.Expressions;
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
            throw new NotImplementedException();
        }

        public override string GetQueryText(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
