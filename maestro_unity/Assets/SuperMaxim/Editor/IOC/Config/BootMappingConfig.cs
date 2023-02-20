using System;
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
        }
    }
}
