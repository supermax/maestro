using System;

namespace SuperMaxim.IOC.Attributes
{
    // TODO review AttributeUsage params
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TypeMapAttribute : Attribute
    {
        public bool IsSingleton { get; set; }
        
        public bool IsLazy { get; set; }
        
        public Type[] MapTypes { get; set; }
    }
}