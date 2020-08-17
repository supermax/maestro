using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SuperMaxim.Core.Extensions;

namespace SuperMaxim.IOC.Container
{
    internal class TypeMapCache
    {
        private readonly IDictionary<Type, ITypeMap> _cache = new ConcurrentDictionary<Type, ITypeMap>();
        
        private readonly IResolver _resolver;

        internal TypeMapCache()
        {
            _resolver = new Resolver(_cache);
        }

        internal ITypeMap<T> Get<T>() where T : class
        {
            var type = typeof(T);
            if (!_cache.ContainsKey(type))
            {
                return null;
            }
            var map = _cache[type];
            return map as ITypeMap<T>;
        }

        internal ITypeMap<T> Set<T>() where T : class
        {
            var map = new TypeMap<T>(_resolver);
            _cache[typeof(T)] = map;
            return map;
        }

        internal void Remove<T>() where T : class
        {
            var type = typeof(T);
            if (!_cache.ContainsKey(type))
            {
                return;
            }

            var map = _cache[type];
            _cache.Remove(type);
            map.Dispose();
        }

        internal void Reset()
        {
            _cache.ForEach(map => map.Value.Dispose());
            _cache.Clear();
        }
    }
}