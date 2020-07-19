using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SuperMaxim.IOC.Container
{
    // TODO add mapType and instance fields for single-mapping and use dic for multi-mapping
    // TODO dispose upon removal from cache
    // TODO refer to TypeMapAttr and to InitTrigger during mapping
    internal class TypeMap<T> : ITypeMap<T>, ITypeMapResolver<T>, ITypeMapReset<T>, IDisposable
    {
        private bool _isSingleton;
        
        private Type _keyType;

        private IDictionary<string, Type> _mapTypes;

        private IDictionary<string, object> _instances;

        public TypeMap(Type keyType)
        {
            _keyType = keyType;
            _mapTypes = new ConcurrentDictionary<string, Type>();
            _instances = new ConcurrentDictionary<string, object>();
        }
        
        public ITypeMap<T> To<TM>(string key = null) where TM : T
        {
            // TODO check if TM is interface
            var type = typeof(TM);
            if (key != null)
            {
                _mapTypes[key] = type;
                return this;
            }
            key = type.Name;
            _mapTypes[key] = type;
            return this;
        }

        public ITypeMapReset<T> From<TM>(string key = null) where TM : T
        {
            throw new NotImplementedException();
        }

        ITypeMapReset<T> ITypeMapReset<T>.Singleton(string key)
        {
            throw new NotImplementedException();
        }

        ITypeMapReset<T> ITypeMapReset<T>.Singleton(T instance, string key)
        {
            throw new NotImplementedException();
        }

        public ITypeMap<T> Singleton<TM>(string key = null) where TM : T
        {
            To<TM>(key);
            _isSingleton = true;
            return this;
        }

        public ITypeMap<T> Singleton<TM>(T instance, string key = null) where TM : T
        {
            Singleton<TM>(key);
            var type = typeof(T);
            if (key != null)
            {
                _instances[key] = type;
                return this;
            }
            key = type.Name;
            _instances[key] = type;
            return this;
        }

        public T Instance(string key = null, params object[] args)
        {
            T instance = default;
            if (!_isSingleton)
            {
                instance = Resolve();
                return instance;
            }
            
            var type = typeof(T);
            if (key != null)
            {
                if (_instances.ContainsKey(key))
                {
                    instance = (T)_instances[key];
                }
                return instance;
            }
            key = type.Name;
            instance = (T)_instances[key];
            return instance;
        }
        
        public T Inject(T instance, params object[] args)
        {
            throw new NotImplementedException();
        }

        private T Resolve()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _keyType = null;
            
            _mapTypes?.Clear();
            _mapTypes = null;
            
            _instances?.Clear();
            _instances = null;
        }
    }
}