using System;

namespace SuperMaxim.IOC.Config
{
    [Serializable]
    public class AssemblyConfig : BaseConfig
    {
        public TypeConfig[] Types
        {
            get;
            set;
        }
    }
}
