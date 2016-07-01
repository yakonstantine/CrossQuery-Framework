using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossQuery.Interfaces;

namespace CrossQuery.Linq.Tests.Mock.DB1_Context
{
    public class DB1Adapter : IDataAdapter
    {
        private DB1Entities context = new DB1Entities();

        public string Name
        {
            get
            {
                return "DB1";
            }
        }

        public IQueryable<TEntity> GetEntities<TEntity>() where TEntity : class
        {
            return context.Set<TEntity>();
        }

        public void AddEntity<TEntity>(TEntity entity) where TEntity : class
        {
            context.Set<TEntity>().Add(entity);
        }

        public void ExecuteSqlCommand(string command)
        {
            context.Database.ExecuteSqlCommand(command);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
