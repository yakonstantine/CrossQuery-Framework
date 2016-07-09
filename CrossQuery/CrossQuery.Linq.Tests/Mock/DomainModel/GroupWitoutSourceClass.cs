using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossQuery.Linq.Attributes;

namespace CrossQuery.Linq.Tests.Mock.DomainModel
{
    [Adapter("DB1", null)]
    public class GroupWitoutSourceClass
    {
        public Guid ID { get; set; }

        public string Name { get; set; }
    }
}
