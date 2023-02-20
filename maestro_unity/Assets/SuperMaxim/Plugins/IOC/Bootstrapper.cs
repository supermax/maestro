using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Config;
using SuperMaxim.Logging;
using UnityEngine;

namespace SuperMaxim.IOC
{
    /// <summary>
    /// Responsible for loading and mapping types into <see cref="Maestro"/> container
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] protected BootConfig _bootConfig;

        private void Awake()
        {
            Setup();
        }

        /// <summary>
        /// Read <see cref="BootConfig"/> and map types in <see cref="Maestro"/>
        /// </summary>
        /// <remarks>Or, auto-load type mappings from program's assemblies is <see cref="BootConfig"/> `AutoLoad` Flag = True</remarks>
        protected virtual void Setup()
        {
            if (_bootConfig == null)
            {
                Loggers.Console.LogInfo($"{nameof(BootConfig)} is null. Skipping {nameof(Setup)}");
            }
            if (_bootConfig.Assemblies.IsNullOrEmpty())
            {
                Loggers.Console.LogInfo($"{nameof(_bootConfig.Assemblies)} is null. Skipping {nameof(Setup)}");
            }

            Loggers.Console.LogInfo("Processing {0}...", _bootConfig);

            foreach (var assemblyConfig in _bootConfig.Assemblies)
            {
                if (assemblyConfig == null)
                {
                    Loggers.Console.LogInfo($"{nameof(AssemblyConfig)} is null. Skipping it.");
                    continue;
                }
                if (assemblyConfig.Types.IsNullOrEmpty())
                {
                    Loggers.Console.LogInfo($"[{assemblyConfig.Name}] {nameof(AssemblyConfig)}->{nameof(assemblyConfig.Types)} is null/empty. Skipping it.");
                    continue;
                }

                foreach (var typeConfig in assemblyConfig.Types)
                {
                    if (typeConfig == null)
                    {
                        Loggers.Console.LogInfo($"{nameof(TypeConfig)} is null. Skipping it.");
                        continue;
                    }

                    Map(typeConfig);
                }
            }

            Loggers.Console.LogInfo("Processed {0}.", _bootConfig);
        }

        private static void Map(TypeConfig typeConfig)
        {
            var maestroInterfaceType = typeof (IMaestro);
            var mapMethodInfo = maestroInterfaceType.GetMethod(nameof(Maestro.Default.Map));
            var genericMapMethodInfo = mapMethodInfo.MakeGenericMethod(typeConfig.SourceType);

            genericMapMethodInfo.Invoke(Maestro.Default, null);

            //Maestro.Default.Map<object>().Singleton<object>();
        }
    }
}
