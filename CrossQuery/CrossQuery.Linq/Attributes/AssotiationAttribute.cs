using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Linq.Attributes
{
    public class AssotiationAttribute : Attribute
    {
        public AssotiationAttribute() { }

        public AssotiationAttribute(string sourcePropertyName, Type targetClass, string targetPropertyName)
        {
            this.SourcePropertyName = sourcePropertyName;
            this.TargetClass = targetClass;
            this.TargetPropertyName = targetPropertyName;
        }

        public string SourcePropertyName { get; set; }

        public Type TargetClass { get; set; }

        public string TargetPropertyName { get; set; }
    }
}
