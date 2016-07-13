using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossQuery.Linq.Attributes;

namespace CrossQuery.Linq.Tests.Mock.DomainModel
{
    [Adapter(null, null)]
    public class GroupAdapterNameNull
    {
        public Guid ID { get; set; }

        public string Name { get; set; }
    }

    [Adapter("", null)]
    public class GroupAdapterNameEmpty
    {
        public Guid ID { get; set; }

        public string Name { get; set; }
    }
}
