using System;
using System.Runtime.Serialization;

namespace SuperMaxim.IOC.Config
{
    [Serializable]
    [DataContract]
    public abstract class BaseConfig
    {
        [DataMember(Name = "name")]
        public virtual string Name
        {
            get;
            set;
        }
    }
}
