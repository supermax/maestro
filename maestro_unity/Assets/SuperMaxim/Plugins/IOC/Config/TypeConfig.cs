using System;
using System.Runtime.Serialization;

namespace SuperMaxim.IOC.Config
{
    [Serializable]
    [DataContract]
    public class TypeConfig : BaseConfig
    {
        [DataMember(Name = "sourceType")]
        public Type SourceType
        {
            get;
            set;
        }

        [DataMember(Name = "typeMappings")]
        public Type[] TypeMappings
        {
            get;
            set;
        }

        [DataMember(Name = "typeDependencies")]
        public Type[] TypeDependencies
        {
            get;
            set;
        }

        [DataMember(Name = "typeDependencies")]
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
