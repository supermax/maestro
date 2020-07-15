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

        public Type TargetType { get; set; }
        
        public Type[] Dependencies { get; set; }

        public TypeConfig()
        {
            //System.Reflection.TypeDelegator d = new TypeDelegator(TargetType);
            
            
        }
    }
}