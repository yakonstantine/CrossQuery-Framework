using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Linq.Tests.Mock.DB_1_Entities
{
    public class Student
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public Group Group { get; set; }
    }
}
