using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CrossQuery.Mapper.Interfaces;
using CrossQuery.Mapper.Internal;

namespace CrossQuery.Mapper
{
    public static class Mapper
    {
        private static List<IMapperConfiguration> _mapperConfigurations = new List<IMapperConfiguration>();

        public static TDest Map<TSource, TDest>(TSource source)  
            where TDest : class, new()
            where TSource : class
        {
            if (source == null)
                throw new NullReferenceException("source is null.");

            var mapperConfiguration = GetConfiguration<TSource, TDest>();

            if (mapperConfiguration == null)
                throw new NotImplementedException($"Mapper for {typeof(TSource).Name} and {typeof(TDest).Name} is not implemented");

            return ((IMapperConfiguration<TSource, TDest>)mapperConfiguration).Map(source);
        }        

        public static IEnumerable<TDest> Map<TSource, TDest>(IQueryable<TSource> sourceCollection)
            where TDest : class, new()
            where TSource : class
        {
            if (sourceCollection == null)
                throw new NullReferenceException("Source Collection is null.");

            var mapperConfiguration = GetConfiguration<TSource, TDest>();

            if (mapperConfiguration == null)
                throw new NotImplementedException($"Mapper for {typeof(TSource).Name} and {typeof(TDest).Name} is not implemented");

            return sourceCollection
                .ToList()
                .Select(s => ((MapperConfiguration<TSource, TDest>)mapperConfiguration).Map(s))
                .AsParallel()
                .ToList();
        }

        public static object Map(Type TSource, Type TDest, object source)
        {
            if (TSource == null)
                throw new NullReferenceException("TSource is null");

            if (TDest == null)
                throw new NullReferenceException("TDest is null");

            if (source == null)
                throw new NullReferenceException("source is null");

            Type sourceType = TSource;
            Type destinationType = TDest;
            Type sourceObjectType = source.GetType();

            bool sourceTypeIsArray = false;

            if (typeof(IEnumerable).IsAssignableFrom(TSource))
            {
                if (!TSource.IsGenericType || !TDest.IsGenericType)
                    throw new ArgumentException("TSource or TDest is non generic array");

                sourceTypeIsArray = true;

                sourceType = sourceType.GetGenericArguments()[0];
                destinationType = destinationType.GetGenericArguments()[0];
                sourceObjectType = sourceObjectType.GetGenericArguments()[0];
            }

            if (!sourceType.IsClass)
                throw new ArgumentException($"{sourceType.Name} is not a class");

            if (!destinationType.IsClass || destinationType.GetConstructor(Type.EmptyTypes) == null)
                throw new ArgumentException($"{destinationType.Name} is not a class or is not inmpemented default constructor");            

            if (sourceObjectType != sourceType)
                throw new ArgumentException($"source is not a {sourceType.Name}");

            var mapperConfiguration = GetConfiguration(sourceType, destinationType);

            if (mapperConfiguration == null)
                throw new NotImplementedException($"Mapper for {sourceType.Name} and {destinationType.Name} is not implemented");

            if (sourceTypeIsArray)
            {
                var genericListType = typeof(List<>).MakeGenericType(new[] { destinationType });
                var desinationList = Activator.CreateInstance(genericListType);

                foreach (var sourceObj in (IEnumerable)source)
                    genericListType.GetMethod("Add").Invoke(desinationList, new[] { mapperConfiguration.Map(sourceObj) });

                return desinationList;
            }

            return mapperConfiguration.Map(source);
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
            return _mapperConfigurations
                .FirstOrDefault(mc => mc.GetDestinationType() == typeof(TDest) && mc.GetSourceType() == typeof(TSource));
        }

        private static IMapperConfiguration GetConfiguration(Type TSource, Type TDest)

        {
            return _mapperConfigurations
                .FirstOrDefault(mc => mc.GetDestinationType() == TDest && mc.GetSourceType() == TSource);
        }
    }
}
