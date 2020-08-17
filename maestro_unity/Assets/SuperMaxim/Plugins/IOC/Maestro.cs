using System;
using System.Collections.Generic;
using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Core.Objects;
using SuperMaxim.IOC.Attributes;
using SuperMaxim.IOC.Container;
using SuperMaxim.IOC.Extensions;

namespace SuperMaxim.IOC
{
    public class Maestro : Singleton<IMaestro, Maestro>, IMaestro, IDisposable
    {
        private readonly TypeMapCache _cache = new TypeMapCache();

        public ITypeMapResolver<T> Get<T>() where T : class
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            var map = _cache.Get<T>() as ITypeMapResolver<T>;
            return map;
        }

        public ITypeMap<T> Map<T>() where T : class
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            var map = _cache.Set<T>();
            return map;
        }

        public ITypeMapReset<T> UnMap<T>() where T : class
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            var map = _cache.Get<T>() as ITypeMapReset<T>;
            return map;
        }

        public void UnMapAll<T>() where T : class
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            _cache.Remove<T>();
        }

        public void Dispose()
        {
            _cache.Reset();
        }
    }
}