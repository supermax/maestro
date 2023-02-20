using System;
using SuperMaxim.IOC.Config;
using UnityEditor;
using UnityEngine;

namespace SuperMaxim.Editor.IOC.Config
{
    [Serializable]
    public class TypeMappingConfig : BaseMappingConfig<TypeConfig>
    {
        [SerializeField]
        private TypeInitTrigger _initTrigger;

        [SerializeField]
        private bool _singleton;

        [SerializeField]
        private MonoScript _type;

        [SerializeField]
        private MonoScript[] _implementations;

        [SerializeField]
        private MonoScript[] _dependencies;

        public override TypeConfig GetConfig()
        {
            throw new NotImplementedException();
        }
    }
}
