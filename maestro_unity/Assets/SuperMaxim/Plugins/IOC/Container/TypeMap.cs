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
    internal class TypeMap<T> : ITypeMap, ITypeMap<T>, ITypeMapResolver<T>, ITypeMapReset<T> where T : class
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
                key = type.FullName;
            }
            if (_defaultMapTypeKey == null)
            {
                _defaultMapTypeKey = key;
            }
            // TODO var mapAtt = src.GetCustomAttribute<TypeMapAttribute>();
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
                key = type.FullName;
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
                key = type.FullName;
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
                key = type.FullName;
            }
            _defaultInstanceKey = key;
            _instances[key] = instance;
            return Singleton<TM>(key);
        }

        public T Instance(string key = null, params object[] args)
        {
            T instance;
            var type = typeof(T);
            var instanceKey = (key ?? _defaultInstanceKey) ?? type.FullName;
            if (_instances.ContainsKey(instanceKey))
            {
                instance = _instances[instanceKey];
                return instance;
            }
            
            var typeKey = (key ?? _defaultMapTypeKey) ?? type.FullName;
            if (!_mapTypes.ContainsKey(typeKey))
            {
                MapType<T>(typeKey);
            }
            else
            {
                type = _mapTypes[typeKey].Type;
            }
            
            instance = Resolve(type, args);
            if (!_mapTypes[typeKey].IsSingleton)
            {
                return instance;
            }
            
            _instances[typeKey] = instance;
            return instance;
        }
        
        public T Inject(T instance, params object[] args)
        {
            // TODO implement
            return default;
        }

        private T Resolve(Type type, params object[] args)
        {
            var instance = _resolver.Resolve<T>(type, args);
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
            return $"{nameof(TypeMap<T>)}<{typeof(T).FullName}>";
        }
    }
}