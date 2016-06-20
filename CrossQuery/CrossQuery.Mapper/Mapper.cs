using System;
using System.Collections.Generic;
using System.Linq;
using CrossQuery.Mapper.Interfaces;
using CrossQuery.Mapper.Internal;

namespace CrossQuery.Mapper
{
    public static class Mapper
    {
        internal static List<IMapperConfiguration> _mapperConfigurations = new List<IMapperConfiguration>();

        public static TDest Map<TSource, TDest>(TSource source)  
            where TDest : class, new()
            where TSource : class
        {
            if (source == null)
                throw new NullReferenceException("source is null.");

            var destination = new TDest();
            var mapperConfiguration = GetConfiguration<TSource, TDest>();

            if (mapperConfiguration == null)
                throw new NotImplementedException($"Mapper for {typeof(TSource).Name} and {typeof(TDest).Name} is not implemented");

            ((MapperConfiguration<TSource, TDest>)mapperConfiguration).Map(source, destination);

            return destination;
        }

        public static IMapperConfiguration<TSource, TDest> CreateConfiguration<TSource, TDest>()
            where TDest : class, new()
            where TSource : class
        {
            var mapperConfiguration = GetConfiguration<TSource, TDest>();

            if (mapperConfiguration == null)
            {
                mapperConfiguration = new MapperConfiguration<TSource, TDest>();
                _mapperConfigurations.Add(mapperConfiguration); 
            }

            return (IMapperConfiguration<TSource, TDest>)mapperConfiguration;
        }

        public static void ClearConfiguration()
        {
            _mapperConfigurations.Clear();
        }

        private static IMapperConfiguration GetConfiguration<TSource, TDest>()
            where TDest : class, new()
            where TSource : class

        {
            return _mapperConfigurations.FirstOrDefault(mc => mc.GetDestinationType() == typeof(TDest) && mc.GetSourceType() == typeof(TSource));
        }
    }
}
