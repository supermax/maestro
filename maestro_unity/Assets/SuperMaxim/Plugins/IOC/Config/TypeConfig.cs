using System;
using System.Runtime.Serialization;

namespace SuperMaxim.IOC.Config
{
    [Serializable]
    [DataContract]
    public class TypeConfig : BaseConfig
    {
        [DataMember(Name = "sourceType")]
        public string SourceType
        {
            get;
            set;
        }

        [DataMember(Name = "typeMappings")]
        public string[] TypeMappings
        {
            get;
            set;
        }

        [DataMember(Name = "typeDependencies")]
        public string[] TypeDependencies
        {
            get;
            set;
        }

        [DataMember(Name = "initTrigger")]
        public TypeInitTrigger InitTrigger
        {
            get;
            set;
        }

        [DataMember(Name = "isSingleton")]
        public bool IsSingleton
        {
            get;
            set;
        }
    }
}
