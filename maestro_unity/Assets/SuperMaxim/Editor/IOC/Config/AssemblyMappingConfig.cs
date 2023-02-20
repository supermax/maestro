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
            throw new NotImplementedException();
        }
    }
}
