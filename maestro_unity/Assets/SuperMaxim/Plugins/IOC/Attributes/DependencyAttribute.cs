using System;

namespace SuperMaxim.IOC.Attributes
{
    // TODO review AttributeUsage params
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class DependencyAttribute : Attribute
    {
        public Type[] Dependencies { get; set; }
    }
}