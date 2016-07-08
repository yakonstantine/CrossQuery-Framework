using System;
using System.Collections.Generic;
using System.Linq;
using CrossQuery.Entity.Interfaces;
using CrossQuery.Interfaces;
using CrossQuery.Linq;
using CrossQuery.Linq.Collections;
using CrossQuery.Linq.Interfaces;
using CrossQuery.Mapper;

namespace CrossQuery.Entity
{
    public class CQContext : ICQAdapter
    {
        private IList<Type> _modelTypes = new List<Type>();
        private CQProvider _provider;
        private Mapper.Mapper _mapper;

        public CQContext(Mapper.Mapper mapper, params IDataAdapter[] dataAdapters)
        {
            _mapper = mapper;
            _provider = new CQProvider(_mapper, dataAdapters);
        }

        public Mapper.Mapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        public IQueryable<TEntity> GetEntities<TEntity>() 
            where TEntity : class, ICQObject
        {
            if (!_modelTypes.Any(t => t == typeof(TEntity)))
                throw new ArgumentException($"Type {typeof(TEntity)} don't added to context model types");

            return new CQSet<TEntity>(_provider);
        }

        public void AddModelType(Type type)
        {
            if (!typeof(ICQObject).IsAssignableFrom(type))
                throw new ArgumentException($"{type.Name} don't implemet ICQObject");

            _modelTypes.Add(type);
        }

        public void RemoveModelType(Type type)
        {
            _modelTypes.Remove(type);
        }
    }
}
