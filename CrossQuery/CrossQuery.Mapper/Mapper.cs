using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CrossQuery.Mapper.Interfaces;
using CrossQuery.Mapper.Internal;

namespace CrossQuery.Mapper
{
    public class Mapper
    {
        private List<IMapperConfiguration> _mapperConfigurations = new List<IMapperConfiguration>();

        public TDest Map<TSource, TDest>(TSource source)  
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

        public IEnumerable<TDest> Map<TSource, TDest>(IEnumerable<TSource> sourceCollection)
            where TDest : class, new()
            where TSource : class
        {
            if (sourceCollection == null)
                throw new NullReferenceException("Source Collection is null.");

            var mapperConfiguration = GetConfiguration<TSource, TDest>();

            if (mapperConfiguration == null)
                throw new NotImplementedException($"Mapper for {typeof(TSource).Name} and {typeof(TDest).Name} is not implemented");

            return sourceCollection
                .Select(s => ((MapperConfiguration<TSource, TDest>)mapperConfiguration).Map(s))
                .AsParallel()
                .ToList();
        }

        public object Map(Type TSource, Type TDest, object source)
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

            bool sourceIsArray = false;

            if (typeof(IEnumerable).IsAssignableFrom(sourceObjectType))
            {
                sourceIsArray = true;
                sourceObjectType = sourceObjectType.GetGenericArguments()[0];
            }

            if (!sourceType.IsClass)
                throw new ArgumentException($"{sourceType.Name} is not a class");

            if (!destinationType.IsClass || destinationType.GetConstructor(Type.EmptyTypes) == null)
                throw new ArgumentException($"{destinationType.Name} is not a class or is not inmpemented default constructor");            

            if (sourceObjectType != sourceType)
                throw new ArgumentException($"Source object is not a {sourceType.Name}");

            var mapperConfiguration = GetConfiguration(sourceType, destinationType);

            if (mapperConfiguration == null)
                throw new NotImplementedException($"Mapper for {sourceType.Name} and {destinationType.Name} is not implemented");

            if (sourceIsArray)
            {
                var genericListType = typeof(List<>).MakeGenericType(new[] { destinationType });
                var desinationList = Activator.CreateInstance(genericListType);

                foreach (var sourceObj in (IEnumerable)source)
                    genericListType.GetMethod("Add").Invoke(desinationList, new[] { mapperConfiguration.Map(sourceObj) });

                return desinationList;
            }

            return mapperConfiguration.Map(source);
        }

        public IMapperConfiguration<TSource, TDest> CreateConfiguration<TSource, TDest>()
            where TDest : class, new()
            where TSource : class
        {
            var mapperConfiguration = GetConfiguration<TSource, TDest>();

            if (mapperConfiguration == null)
            {
                mapperConfiguration = new MapperConfiguration<TSource, TDest>(this);
                _mapperConfigurations.Add(mapperConfiguration); 
            }

            return (IMapperConfiguration<TSource, TDest>)mapperConfiguration;
        }

        private IMapperConfiguration GetConfiguration<TSource, TDest>()
            where TDest : class, new()
            where TSource : class

        {
            return _mapperConfigurations
                .FirstOrDefault(mc => mc.GetDestinationType() == typeof(TDest) && mc.GetSourceType() == typeof(TSource));
        }

        private IMapperConfiguration GetConfiguration(Type TSource, Type TDest)

        {
            return _mapperConfigurations
                .FirstOrDefault(mc => mc.GetDestinationType() == TDest && mc.GetSourceType() == TSource);
        }
    }
}
