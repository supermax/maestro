using System;
using System.Collections.Generic;
using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Attributes;
using SuperMaxim.IOC.Container;
using SuperMaxim.IOC.Extensions;

namespace SuperMaxim.IOC
{
    public sealed class Maestro : IMaestro, IDisposable
    {
        private static readonly Maestro DefaultInstance = new Maestro();

        public static IMaestro Default => DefaultInstance;
        
        private readonly TypeMapCache _cache = new TypeMapCache();

        private Maestro()
        {
        }
        
        public ITypeMapResolver<T> Get<T>()
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            var map = _cache.Get<T>() as ITypeMapResolver<T>;
            return map;
        }

        public ITypeMap<T> Map<T>()
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            var map = _cache.Set<T>();
            return map;
        }

        public ITypeMapReset<T> UnMap<T>()
        {
            var type = typeof(T);
            if (!type.IsClass && !type.IsInterface)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            var map = _cache.Get<T>() as ITypeMapReset<T>;
            return map;
        }

        public void UnMapAll<T>()
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