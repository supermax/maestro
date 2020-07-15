using System;
using System.Data;
using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Attributes;
using SuperMaxim.IOC.Extensions;
using Object = UnityEngine.Object;

namespace SuperMaxim.IOC
{
    public class DIMaestro : IMaestro
    {
        public T Resolve<T>(string key = null, params object[] args)
        {
            // TODO
            return default;
        }
        
        private object Instantiate(Type src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            if (src.IsAbstract)
            {
                throw new OperationCanceledException($"The {src} is an abstract class");
            }
            if (src.IsInterface)
            {
                throw new OperationCanceledException($"The {src} is an interface");
            }

            var mapAtt = src.GetCustomAttribute<TypeMapAttribute>();
            if (mapAtt != null && mapAtt.IsSingleton)
            {
                // TODO get from cache and return
                return null;
            }

            var depAtt = src.GetCustomAttribute<DependencyAttribute>();
            if (depAtt != null && !depAtt.Dependencies.IsNullOrEmpty())
            {
                foreach (var dependency in depAtt.Dependencies)
                {
                    Instantiate(dependency);
                }
            }

            var ctor = src.GetDefaultConstructor();
            if (ctor == null)
            {
                throw new OperationCanceledException($"Cannot get default constructor for {src}");
            }

            object instance = null;
            var ctorParams = ctor.GetParameters();
            if (ctorParams.IsNullOrEmpty())
            {
                instance = ctor.Invoke(null);
            }
            return instance;
        }
    }
}