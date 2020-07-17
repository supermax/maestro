using System;

namespace SuperMaxim.IOC.Attributes
{
    // TODO review AttributeUsage params
    [AttributeUsage(AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    {
        // TODO use custom ID
        public override object TypeId { get; }
    }
}