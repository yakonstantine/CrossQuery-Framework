using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossQuery.Interfaces;

namespace CrossQuery.Linq.Tests.Mock.DB2_Context
{
    public class DB2Adapter : IDataAdapter
    {
        private DB2Entities context = new DB2Entities();

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
