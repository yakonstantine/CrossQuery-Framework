using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossQuery.Interfaces;
using CrossQuery.Linq.Tests.Mock.DB_1_Entities;

namespace CrossQuery.Linq.Tests.Mock
{
    public class DB_1_Adapter : IDataAdapter
    {
        List<Group> _groups = new List<Group>();
        List<Student> _students = new List<Student>();

        public string Name
        {
            get
            {
                return "DB1";
            }
        }

        public IQueryable<TEntity> GetEntities<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void AddObjectToCollection<T>(T obj) where T : class
        {
            if (typeof(T) == typeof(Group))
            {
                _groups.Add((Group)(object)obj);
                return;
            }

            if (typeof(T) == typeof(Student))
            {
                _students.Add((Student)(object)obj);
                return;
            }

            throw new NotImplementedException();
        }
    }
}
