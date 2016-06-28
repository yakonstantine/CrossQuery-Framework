using System;

namespace CrossQuery.Linq.Attributes
{
    public class AdapterAttribute : Attribute
    {
        public AdapterAttribute() { }

        public AdapterAttribute(string adapterName)
        {
            this.AdapterName = adapterName;
        }

        public string AdapterName { get; set; }
    }
}
