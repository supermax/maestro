using System;
using SuperMaxim.Core.Extensions;
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
            // TODO use type's full name and not just simple name

            var config = new TypeConfig
                {
                    Name = Name,
                    InitTrigger = _initTrigger,
                    IsSingleton = _singleton,
                    SourceType = _type.name,
                };

            if (!_implementations.IsNullOrEmpty())
            {
                config.TypeMappings = new string[_implementations.Length];
                for (var i = 0; i < _implementations.Length; i++)
                {
                    config.TypeMappings[i] = _implementations[i].name;
                }
            }
            if (_dependencies.IsNullOrEmpty())
            {
                return config;
            }

            config.TypeDependencies = new string[_dependencies.Length];
            for (var i = 0; i < _dependencies.Length; i++)
            {
                config.TypeDependencies[i] = _dependencies[i].name;
            }
            return config;
        }
    }
}
