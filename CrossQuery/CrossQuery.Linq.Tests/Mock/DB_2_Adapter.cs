using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossQuery.Interfaces;
using CrossQuery.Linq.Tests.Mock.DB_2_Entities;

namespace CrossQuery.Linq.Tests.Mock
{
    public class DB_2_Adapter : IDataAdapter
    {
        List<Event> _events = new List<Event>();

        public string Name
        {
            get
            {
                return "DB2";
            }
        }

        public IQueryable<TEntity> GetEntities<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void AddObjectToCollection<T>(T obj) where T : class
        {
            if (typeof(T) == typeof(Event))
            {
                _events.Add((Event)(object)obj);
                return;
            }

            throw new NotImplementedException();
        }
    }
}
