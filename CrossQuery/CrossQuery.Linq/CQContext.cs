using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossQuery.Interfaces;
using CrossQuery.Linq.Attributes;
using CrossQuery.Linq.Collections;

namespace CrossQuery.Linq
{
    public class CQContext : IDataAdapter
    {
        private Mapper.Mapper _mapper;
        private CQProvider _provider;

        public CQContext(Mapper.Mapper mapper, params IDataAdapter[] dataAdapters)
        {
            _mapper = mapper;
            _provider = new CQProvider(_mapper, dataAdapters);
        }

        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public Mapper.Mapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        public void AddEntity<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetEntities<TEntity>() where TEntity : class
        {
            if (typeof(TEntity).CustomAttributes.Any(a => a.AttributeType == typeof(AdapterAttribute)))
                throw new ArgumentException($"{typeof(TEntity).Name} does not contain attribute {typeof(AdapterAttribute).Name}");

            return new CQSet<TEntity>(_provider);
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
