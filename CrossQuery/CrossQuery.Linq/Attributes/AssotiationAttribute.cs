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

        public AssotiationAttribute(string sourcePropertyName, string targetClassName, string targetPropertyName)
        {
            this.SourcePropertyName = sourcePropertyName;
            this.TargetClassName = targetClassName;
            this.TargetPropertyName = targetPropertyName;
        }

        public string SourcePropertyName { get; set; }

        public string TargetClassName { get; set; }

        public string TargetPropertyName { get; set; }
    }
}
