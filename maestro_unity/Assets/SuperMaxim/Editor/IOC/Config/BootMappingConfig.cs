using System;
using System.Linq;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Config;
using UnityEngine;

namespace SuperMaxim.Editor.IOC.Config
{
    [Serializable]
    [CreateAssetMenu(fileName = "BootConfig", menuName = "Boot Config")]
    public class BootMappingConfig : BootConfig
    {
        [SerializeField] private bool _autoConfig;

        [SerializeField] private AssemblyMappingConfig[] _assemblies;

        private void OnValidate()
        {
            Debug.Log(nameof(OnValidate));

            AutoConfig = _autoConfig;
            Assemblies = _assemblies.Select(assemblyMapping => assemblyMapping.GetConfig()).ToArray();
            var json = Assemblies.ToJson();
            Debug.Log(json);
        }
    }
}
