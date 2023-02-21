using System;
using System.Runtime.Serialization;

namespace SuperMaxim.IOC.Config
{
    [Serializable]
    [DataContract]
    public enum TypeInitTrigger
    {
        [EnumMember(Value = "onDemand")]
        OnDemand,

        [EnumMember(Value = "onMapping")]
        OnMapping
    }
}
