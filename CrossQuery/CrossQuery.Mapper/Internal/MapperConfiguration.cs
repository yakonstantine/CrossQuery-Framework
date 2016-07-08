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
        private Mapper _mapper;

        internal MapperConfiguration(Mapper mapper)
        {
            _mapper = mapper;
        }

        public Mapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        internal void AddNewOperation(Func<TSource, object> getSourcePropertyValue, Expression<Func<TDest, object>> destinationProperyExpression)
        {
            _mapOperations.Add(new MapOperation<TSource, TDest>()
            {
                MapperConfiguration = this,
                DestinationProperyExpression = destinationProperyExpression,
                GetSourcePropertyValue = getSourcePropertyValue
            });
        }

        object IMapperConfiguration.Map(object source)
        {
            dynamic detinationObj = Activator.CreateInstance(GetDestinationType());
            dynamic sourceObj = source;

            foreach (var mapOperation in _mapOperations)
                mapOperation.Map(sourceObj, detinationObj);

            return detinationObj;
        }

        public TDest Map(TSource sourceObj) 
        {
            return (TDest)((IMapperConfiguration)this).Map(sourceObj);
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
