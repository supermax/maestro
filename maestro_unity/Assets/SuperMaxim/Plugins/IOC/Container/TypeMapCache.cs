using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SuperMaxim.IOC.Container
{
    internal class TypeMapCache
    {
        private readonly IDictionary<Type, ITypeMap> _cache = new ConcurrentDictionary<Type, ITypeMap>();

        internal ITypeMap<T> Get<T>()
        {
            var type = typeof(T);
            if (!_cache.ContainsKey(type))
            {
                return null;
            }
            var map = _cache[type];
            return map as ITypeMap<T>;
        }

        internal ITypeMap<T> Set<T>()
        {
            var map = new TypeMap<T>();
            _cache[typeof(T)] = map;
            return map;
        }

        internal void Remove<T>()
        {
            var type = typeof(T);
            if (!_cache.ContainsKey(type))
            {
                // TODO write to log
                return;
            }
            _cache.Remove(type);
        }

        internal void Reset()
        {
            _cache.Clear();
        }
    }
}