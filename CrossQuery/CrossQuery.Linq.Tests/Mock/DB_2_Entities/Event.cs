﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Linq.Tests.Mock.DB_2_Entities
{
    public class Event
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }
    }
}
