using System;
using System.Linq.Expressions;
using CrossQuery.Mapper.Interfaces;
using CrossQuery.Mapper.Internal;

namespace CrossQuery.Mapper.Extensions
{
    public static class MapperConfigurationExtensions
    {
        public static IMapperConfiguration<TSource, TDest> AddMap<TSource, TDest>(this IMapperConfiguration<TSource, TDest> mapperConfiguration, Func<TSource, object> getSourcePropertyValue, Expression<Func<TDest, object>> destinationProperyExpression)
            where TDest : class, new()
            where TSource : class
        {
            ((MapperConfiguration<TSource, TDest>)mapperConfiguration).AddNewOperation(getSourcePropertyValue, destinationProperyExpression);

            return mapperConfiguration;
        }
    }
}
