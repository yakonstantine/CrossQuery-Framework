using System;
using CrossQuery.Linq.Attributes;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq.Tests.Mock.DomainModel
{
    [Adapter(AdapterName = "DB2", SourceClass = typeof(DB2_Context.Event))]
    public class Event
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        [Assotiation(SourcePropertyName = "GroupName", TargetClass = typeof(Group), TargetPropertyName = "Name" )]
        public Group Group { get; set; }
    }
}
