﻿using System;
using CrossQuery.Linq.Attributes;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq.Tests.Mock.DomainModel
{
    [Adapter("DB1", typeof(DB1_Context.Student))]
    public class Student
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public Group Group { get; set; }
    }
}
