using System;
using CrossQuery.Linq.Attributes;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq.Tests.Mock.DomainModel
{
    [Adapter("DB2", typeof(DB2_Context.Event))]
    public class Event
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        [Assotiation("GroupName", typeof(Group), "Name" )]
        public Group Group { get; set; }
    }
}
