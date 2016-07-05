using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Mapper.Tests.Mock
{
    public class MockDestinationWithoutDefaultConstructor
    {
        public MockDestinationWithoutDefaultConstructor(Guid guidProp, string stringProp, int intProp)
        {
            this.GuidProp = guidProp;
            this.StringProp = stringProp;
            this.IntProp = intProp;
        }

        public Guid GuidProp { get; set; }

        public string StringProp { get; set; }

        public int IntProp { get; set; }
    }
}
