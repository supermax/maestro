using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperMaxim.IOC.Config
{
    [Serializable]
    [CreateAssetMenu(fileName = "BootConfig", menuName = "Boot Config")]
    public class BootConfig : ScriptableObject
    {
        // TODO implement

        [SerializeField] private AssemblyConfig _assemblyConfig;
    }
}