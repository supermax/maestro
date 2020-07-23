using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SuperMaxim.IOC.Container
{
    // TODO add mapType and instance fields for single-mapping and use dic for multi-mapping
    // TODO dispose upon removal from cache
    // TODO refer to TypeMapAttr and to InitTrigger during mapping
    internal class TypeMap<T> : ITypeMap, ITypeMap<T>, ITypeMapResolver<T>, ITypeMapReset<T>, IDisposable
    {
        private bool _isSingleton;

        private string _defaultMapTypeKey;
        
        private IDictionary<string, Type> _mapTypes;

        private string _defaultInstanceKey;
        
        private IDictionary<string, T> _instances;

        internal TypeMap()
        {
            _mapTypes = new ConcurrentDictionary<string, Type>();
            _instances = new ConcurrentDictionary<string, T>();
        }
        
        public ITypeMap<T> To<TM>(string key = null) where TM : class, T
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
            _mapTypes[key] = type;
            return this;
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
            To<TM>(key);
            _isSingleton = true;
            return this;
        }

        public ITypeMap<T> Singleton<TM>(TM instance, string key = null) where TM : class, T
        {
            Singleton<TM>(key);
            var type = typeof(TM);
            if (key == null)
            {
                key = type.Name;
            }
            _defaultInstanceKey = key;
            _instances[key] = instance;
            return this;
        }

        public T Instance(string key = null, params object[] args)
        {
            T instance;
            if (!_isSingleton)
            {
                instance = Resolve(key ?? _defaultMapTypeKey, args);
                return instance;
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

            var implType = _mapTypes[key];
            var dependencies = new List<Type>();
            var instance = (T)Resolver.Resolve(implType, args, dependencies);
            if (_isSingleton)
            {
                _instances[key] = instance;
            }
            return instance;
        }

        public void Dispose()
        {
            _mapTypes?.Clear();
            _mapTypes = null;
            _defaultMapTypeKey = null;
            
            _instances?.Clear();
            _instances = null;
            _defaultInstanceKey = null;
        }
    }
}