using System;
using System.Reflection;
using SuperMaxim.IOC.Attributes;
using UnityEngine;

namespace SuperMaxim.IOC.Config
{
    [Serializable]
    public class TypeConfig
    {
        [TypeDrawer]
        [SerializeField]
        private System.Reflection.TypeDelegator _type;

        public Type SourceType { get; set; }
        
        public Type[] TypeMappings { get; set; }
        
        public Type[] Dependencies { get; set; }
        
        public TypeInitTrigger InitTrigger { get; set; }

        public TypeConfig()
        {
            //System.Reflection.TypeDelegator d = new TypeDelegator(TargetType);
            
            
        }
    }
}