﻿using System;
using CrossQuery.Linq.Attributes;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq.Tests.Mock.DomainModel
{
    [Adapter(AdapterName = "DB1", SourceClass = typeof(DB_1_Entities.Student))]
    public class Student : ICQObject
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public Group Group { get; set; }
    }
}
