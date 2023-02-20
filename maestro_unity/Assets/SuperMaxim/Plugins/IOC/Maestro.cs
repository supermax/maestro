using System;
using SuperMaxim.Core.Objects;
using SuperMaxim.IOC.Container;

namespace SuperMaxim.IOC
{
    public class Maestro : Singleton<IMaestro, Maestro>, IMaestro, IDisposable
    {
        private readonly TypeMapCache _cache = new();

        public ITypeMapResolver<T> Get<T>() where T : class
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class and not an interface!");
            }
            var map = _cache.Get<T>() as ITypeMapResolver<T>;
            return map;
        }

        public ITypeMap<T> Map<T>() where T : class
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class and not an interface!");
            }
            if (type == GetType())
            {
                throw new InvalidOperationException($"The type {type} cannot be {GetType()}!");
            }
            if (type == typeof (IMaestro))
            {
                throw new InvalidOperationException($"The type {type} cannot be {typeof(IMaestro)}!");
            }
            var map = _cache.Set<T>();
            return map;
        }

        public ITypeMapReset<T> UnMap<T>() where T : class
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class and not an interface!");
            }
            var map = _cache.Get<T>() as ITypeMapReset<T>;
            return map;
        }

        public void UnMapAll<T>() where T : class
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class and not an interface!");
            }
            _cache.Remove<T>();
        }

        public void Dispose()
        {
            _cache.Reset();
        }
    }
}
