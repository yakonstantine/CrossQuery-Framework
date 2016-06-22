using System.Linq;
using CrossQuery.Interfaces;

namespace CrossQuery.Entity.Interfaces
{
    public interface ICQAdapter
    {
        IQueryable<TEntity> GetEntities<TEntity>() 
            where TEntity : class, ICQObject;
    }
}
