using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CrossQuery.Mapper.Interfaces;
using CrossQuery.Mapper.Internal;

namespace CrossQuery.Mapper.Internal
{
    internal class MapperConfiguration<TSource, TDest> : IMapperConfiguration<TSource, TDest>
        where TDest : class, new()
        where TSource : class
    {
        private List<MapOperation<TSource, TDest>> _mapOperations = new List<MapOperation<TSource, TDest>>();

        internal void AddNewOperation(Func<TSource, object> getSourcePropertyValue, Expression<Func<TDest, object>> destinationProperyExpression)
        {
            _mapOperations.Add(new MapOperation<TSource, TDest>()
            {
                DestinationProperyExpression = destinationProperyExpression,
                GetSourcePropertyValue = getSourcePropertyValue
            });
        }

        internal void Map(TSource sourceObj, TDest destObj) 
        {
            foreach (var mapOperation in _mapOperations)
                mapOperation.Map(sourceObj, destObj);
        }

        public Type GetDestinationType()
        {
            return this.GetType().GetGenericArguments()[1];
        }

        public Type GetSourceType()
        {
            return this.GetType().GetGenericArguments()[0];
        }
    }
}
