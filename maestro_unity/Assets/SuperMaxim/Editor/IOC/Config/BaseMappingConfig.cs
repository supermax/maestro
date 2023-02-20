using UnityEngine;

namespace SuperMaxim.Editor.IOC.Config
{
    public abstract class BaseMappingConfig<T>
    {
        [SerializeField]
        protected string Name;

        public abstract T GetConfig();
    }
}
