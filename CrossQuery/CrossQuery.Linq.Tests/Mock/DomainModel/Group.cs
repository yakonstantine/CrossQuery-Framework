﻿using System;
using System.Collections.Generic;
using CrossQuery.Linq.Attributes;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq.Tests.Mock.DomainModel
{
    [Adapter(AdapterName = "DB1", SourceClass = typeof(DB_1_Entities.Group))]
    public class Group : ICQObject
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public List<Student> Students { get; set; }
    }
}
