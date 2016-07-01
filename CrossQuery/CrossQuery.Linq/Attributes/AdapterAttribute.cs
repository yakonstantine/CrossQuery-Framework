using System;

namespace CrossQuery.Linq.Attributes
{
    public class AdapterAttribute : Attribute
    {
        public AdapterAttribute() { }

        public AdapterAttribute(string adapterName, Type sourceClass)
        {
            this.AdapterName = adapterName;
            this.SourceClass = sourceClass;
        }

        public string AdapterName { get; set; }

        public Type SourceClass { get; set; }
    }
}
