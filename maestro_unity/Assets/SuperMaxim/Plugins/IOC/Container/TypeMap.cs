using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Attributes;
using SuperMaxim.IOC.Extensions;

namespace SuperMaxim.IOC.Container
{
    // TODO add mapType and instance fields for single-mapping and use dic for multi-mapping
    // TODO dispose upon removal from cache
    // TODO refer to TypeMapAttr and to InitTrigger during mapping
    // TODO handle default keys (add/remove/update)
    internal class TypeMap<T> : ITypeMap, ITypeMap<T>, ITypeMapResolver<T>, ITypeMapReset<T>
    {
        private string _defaultMapTypeKey;
        
        private IDictionary<string, TypeMapConfig> _mapTypes;

        private string _defaultInstanceKey;
        
        private IDictionary<string, T> _instances;

        private IResolver _resolver;

        internal TypeMap(IResolver resolver)
        {
            _resolver = resolver;
            _mapTypes = new ConcurrentDictionary<string, TypeMapConfig>();
            _instances = new ConcurrentDictionary<string, T>();
        }
        
        private ITypeMap<T> MapType<TM>(string key = null, bool isSingleton = false) where TM : class, T
        {
            var type = typeof(TM);
            if (!type.IsClass)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            if (key == null)
            {
                key = type.Name;
            }
            if (_defaultMapTypeKey == null)
            {
                _defaultMapTypeKey = key;
            }
            _mapTypes[key] = new TypeMapConfig {Type = type, IsSingleton = isSingleton};
            return this;
        }
        
        public ITypeMap<T> To<TM>(string key = null) where TM : class, T
        {
            return MapType<TM>(key);
        }

        public ITypeMapReset<T> From<TM>(string key = null) where TM : class, T
        {
            var type = typeof(TM);
            if (!type.IsClass)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            if (key == null)
            {
                key = type.Name;
            }
            _mapTypes.Remove(key);
            if (_defaultMapTypeKey == key)
            {
                _defaultMapTypeKey = null;
            }
            return this;
        }

        public ITypeMapReset<T> From<TM>(TM instance, string key = null) where TM : class, T
        {
            var type = typeof(TM);
            if (!type.IsClass)
            {
                throw new OperationCanceledException($"The type {type} is not a class!");
            }
            if (key == null)
            {
                key = type.Name;
            }
            _instances.Remove(key);
            if (_defaultInstanceKey == key)
            {
                _defaultInstanceKey = null;
            }
            return this;
        }

        public ITypeMap<T> Singleton<TM>(string key = null) where TM : class, T
        {
            return MapType<TM>(key, true);
        }

        public ITypeMap<T> Singleton<TM>(TM instance, string key = null) where TM : class, T
        {
            var type = typeof(TM);
            if (key == null)
            {
                key = type.Name;
            }
            _defaultInstanceKey = key;
            _instances[key] = instance;
            return Singleton<TM>(key);
        }

        public T Instance(string key = null, params object[] args)
        {
            T instance;
            key = key ?? _defaultMapTypeKey;
            if (_mapTypes.ContainsKey(key))
            {
                if (_mapTypes[key].IsSingleton)
                {
                    instance = Resolve(key ?? _defaultMapTypeKey, args);
                    return instance;
                }
            }

            if (key == null)
            {
                key = _defaultInstanceKey;
            }
            if (_instances.ContainsKey(key))
            {
                instance = _instances[key];
                return instance;
            }
            
            instance = Resolve(key, args);
            _instances[key] = instance;
            return instance;
        }
        
        public T Inject(T instance, params object[] args)
        {
            // TODO implement
            return default;
        }

        private T Resolve(string key = null, params object[] args)
        {
            if (key == null)
            {
                key = _defaultMapTypeKey;
            }
            if (key == null)
            {
                var type = typeof(T);
                key = type.Name;
            }

            var typeConfig = _mapTypes[key];
            var instance = _resolver.Resolve<T>(typeConfig.Type, args);
            if (typeConfig.IsSingleton)
            {
                _instances[key] = instance;
            }
            return instance;
        }
        
        public void Dispose()
        {
            _resolver = null;
            
            _mapTypes?.Clear();
            _mapTypes = null;
            _defaultMapTypeKey = null;
            
            _instances?.Clear();
            _instances = null;
            _defaultInstanceKey = null;
        }

        public override string ToString()
        {
            return $"{nameof(TypeMap<T>)}<{typeof(T).Name}>";
        }
    }
}