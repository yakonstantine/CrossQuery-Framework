using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Linq.Tests.Mock.DB_1_Entities
{
    public class Group
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
    }
}
