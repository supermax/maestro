using System;
using System.IO;
using System.Linq;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Config;
using UnityEditor;
using UnityEngine;

namespace SuperMaxim.Editor.IOC.Config
{
    [Serializable]
    [CreateAssetMenu(fileName = "BootConfig", menuName = "Boot Config")]
    public class BootMappingConfig : BootConfig
    {
        [SerializeField] private bool _autoConfig;

        [SerializeField] private TextAsset _configJson;

        [SerializeField] private AssemblyMappingConfig[] _assemblies;

        private void OnValidate()
        {
            Debug.Log(nameof(OnValidate));

            AutoConfig = _autoConfig;
            Assemblies = _assemblies.Select(assemblyMapping => assemblyMapping.GetConfig()).ToArray();
            var json = Assemblies.ToJson();
            Debug.Log(json);

            string configFilePath;
            if (_configJson == null)
            {
                _configJson = new TextAsset();
                configFilePath = AssetDatabase.GetAssetPath(_configJson);
                configFilePath = Path.ChangeExtension(configFilePath, "json");
            }
            else
            {
                configFilePath = AssetDatabase.GetAssetPath(_configJson);
            }
            File.WriteAllText(configFilePath, json);
        }
    }
}
