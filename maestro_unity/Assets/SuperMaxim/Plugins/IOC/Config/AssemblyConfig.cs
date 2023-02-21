using System;
using System.Runtime.Serialization;

namespace SuperMaxim.IOC.Config
{
    [Serializable]
    [DataContract]
    public class AssemblyConfig : BaseConfig
    {
        [DataMember(Name = "types")]
        public TypeConfig[] Types
        {
            get;
            set;
        }
    }
}
