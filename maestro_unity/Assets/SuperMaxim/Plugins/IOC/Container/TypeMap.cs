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

        private List<Type> _dependencies;

        private TypeMapCache _cache;

        internal TypeMap(TypeMapCache cache)
        {
            _cache = cache;
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
            var typeKey = key ?? type.FullName;
            if (_defaultMapTypeKey == null)
            {
                _defaultMapTypeKey = typeKey;
            }

            var mapAtt = type.GetCustomAttribute<TypeMapAttribute>();
            if (mapAtt != null && mapAtt.IsSingleton)
            {
                isSingleton = true;
                //mapAtt.MapTypes // TODO map subtypes
                //mapAtt.InitTrigger // TODO refer to trigger
            }

            var depAtt = type.GetCustomAttribute<DependencyAttribute>();
            if (depAtt != null && !depAtt.Dependencies.IsNullOrEmpty())
            {
                foreach (var dependency in depAtt.Dependencies)
                {
                    // TODO map type
                    // TODO check for circular dependency
                    //Resolve(dependency, args, dependencies);
                    //_cache.
                }
            }

            _mapTypes[typeKey] = new TypeMapConfig {Type = type, IsSingleton = isSingleton};
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

            instance = (T)Resolve(type, args);
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

        public void Dispose()
        {
            _cache = null;

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

        public new Type GetType()
        {
            return typeof (T);
        }

        Type ITypeMap.GetMappedType()
        {
            return _mapTypes[_defaultMapTypeKey].Type;
        }

        // TODO split into short methods
        // TODO use args
        // TODO move back to TypeMap?
        private object Resolve(Type src, object[] args)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            if (!src.IsClass)
            {
                var type = _cache.Get(src);
                if (type == null)
                {
                    throw new OperationCanceledException($"The {src} is not a class!");
                }
                src = type.GetMappedType();
            }

            object instance;

            var depAtt = src.GetCustomAttribute<DependencyAttribute>();
            if (depAtt != null && !depAtt.Dependencies.IsNullOrEmpty())
            {
                _dependencies ??= new List<Type>();
                foreach (var dependency in depAtt.Dependencies)
                {
                    // TODO check for circular dependency
                    if (_dependencies.Contains(dependency))
                    {
                        // TODO write to log
                        continue;
                    }
                    _dependencies.Add(dependency);
                    Resolve(dependency, args);
                }
            }

            var ctor = src.GetDefaultConstructor();
            if (ctor == null)
            {
                throw new OperationCanceledException($"Cannot get constructor for {src}");
            }

            var ctorParams = ctor.GetParameters();
            if (ctorParams.IsNullOrEmpty())
            {
                instance = ctor.Invoke(null);
            }
            else
            {
                var ctorParamValues = new object[ctorParams.Length];
                for (var  i = 0; i < ctorParams.Length; i++)
                {
                    var ctorParam = ctorParams[i];
                    var ctorParamType = ctorParam.ParameterType;
                    var ctorParamValue = Resolve(ctorParamType, args);
                    ctorParamValues[i] = ctorParamValue;
                }
                instance = ctor.Invoke(ctorParamValues);
            }

            var props = src.GetInjectableProperties();
            if (!props.IsNullOrEmpty())
            {
                foreach (var prop in props)
                {
                    if (!prop.CanWrite)
                    {
                        // TODO write to log
                        continue;
                    }

                    var propValue = Resolve(prop.PropertyType, args);
                    prop.SetValue(instance, propValue);
                }
            }

            var methods = src.GetExecutableMethods();
            if (methods.IsNullOrEmpty())
            {
                return instance;
            }

            foreach (var method in methods)
            {
                var methodParams = method.GetParameters();
                if (!methodParams.IsNullOrEmpty())
                {
                    var methodParamsAry = new object[methodParams.Length];
                    for (var i = 0; i < methodParams.Length; i++)
                    {
                        var methodParam = methodParams[i];
                        var paramType = methodParam.ParameterType;
                        var paramInstance = Resolve(paramType, args);
                        methodParamsAry[i] = paramInstance;
                    }
                    method.Invoke(instance, methodParamsAry);
                }
                else
                {
                    method.Invoke(instance, null);
                }
            }
            return instance;
        }
    }
}
