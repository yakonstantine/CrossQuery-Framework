using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Interfaces
{
    public interface IDataAdapter
    {
        string Name { get; }

        IQueryable<TEntity> GetEntities<TEntity>()
            where TEntity : class;

        void AddEntity<TEntity>(TEntity entity)
            where TEntity : class;

        void SaveChanges();
    }
}
