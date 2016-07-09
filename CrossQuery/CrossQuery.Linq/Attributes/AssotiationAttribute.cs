using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Linq.Attributes
{
    public class AssotiationAttribute : Attribute
    {
        public AssotiationAttribute(string sourcePropertyName, Type targetClass, string targetPropertyName)
        {
            this.SourcePropertyName = sourcePropertyName;
            this.TargetClass = targetClass;
            this.TargetPropertyName = targetPropertyName;
        }

        public string SourcePropertyName { get; private set; }

        public Type TargetClass { get; private set; }

        public string TargetPropertyName { get; private set; }
    }
}
