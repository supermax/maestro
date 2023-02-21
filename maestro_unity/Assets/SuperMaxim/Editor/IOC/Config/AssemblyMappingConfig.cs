using System;
using SuperMaxim.IOC.Config;
using UnityEditorInternal;
using UnityEngine;

namespace SuperMaxim.Editor.IOC.Config
{
    [Serializable]
    public class AssemblyMappingConfig : BaseMappingConfig<AssemblyConfig>
    {
        [SerializeField] private AssemblyDefinitionAsset _assembly;

        [SerializeField] private TypeMappingConfig[] _types;

        public override AssemblyConfig GetConfig()
        {
            Name = _assembly.name;
            var config = new AssemblyConfig
                {
                    Name = _assembly.name,
                    Types = new TypeConfig[_types.Length]
                };
            return config;
        }
    }
}
