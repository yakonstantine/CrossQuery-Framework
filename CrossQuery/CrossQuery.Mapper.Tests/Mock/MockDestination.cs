using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Mapper.Tests.Mock
{
    public class MockDestination
    {
        public Guid GuidProp { get; set; }

        public string StringProp { get; set; }

        public int IntProp { get; set; }

        public DateTime DateTimeProp { get; set; }

        public double DoubleProp { get; set; }

        public string ReadOnlyProp { get; private set; }

        public string GetString()
        {
            return this.StringProp;
        }

        public MockDestination1 ReferenceProperty { get; set; }

        public IEnumerable<MockDestination1> CollectionOfReferenceProperties { get; set; }
    }
}
