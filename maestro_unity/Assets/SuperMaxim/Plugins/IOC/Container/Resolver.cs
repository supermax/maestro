using System;
using System.Collections.Generic;
using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Attributes;
using SuperMaxim.IOC.Extensions;

namespace SuperMaxim.IOC.Container
{
    internal class Resolver : IResolver
    {
        private readonly IDictionary<Type, ITypeMap> _cache; // TODO use cache in resolving instances methods
        
        internal Resolver(IDictionary<Type, ITypeMap> cache)
        {
            _cache = cache;
        }

        public T Resolve<T>(Type type, object[] args)
        {
            var dependencies = new List<Type>(); // TODO optimise this
            var instance = Resolve<T>(type, args, dependencies);
            return instance;
        }

        // TODO split into short methods
        // TODO use args
        private T Resolve<T>(Type src, object[] args, ICollection<Type> dependencies)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            if (!src.IsClass)
            {
                throw new OperationCanceledException($"The {src} is not a class!");
            }

            T instance;
            var mapAtt = src.GetCustomAttribute<TypeMapAttribute>();
            if (mapAtt != null && mapAtt.IsSingleton)
            {
                // TODO avoid cyclic loop _cache -> type map -> _cache
                // var key = typeof(T);
                // if (_cache.ContainsKey(key))
                // {
                //     if (_cache[key] is ITypeMapResolver<T> map)
                //     { 
                //         instance = map.Instance(args: args);
                //         if (!Equals(instance, default))
                //         {
                //             return instance;
                //         }
                //     }
                //     
                //     map = _cache[src] as ITypeMapResolver<T>;
                //     if (map != null)
                //     {
                //         
                //     }
                // }
                return default;
            }

            var depAtt = src.GetCustomAttribute<DependencyAttribute>();
            if (depAtt != null && !depAtt.Dependencies.IsNullOrEmpty())
            {
                foreach (var dependency in depAtt.Dependencies)
                {
                    // TODO check for circular dependency
                    if (dependencies.Contains(dependency))
                    {
                        // TODO write to log
                        continue;
                    }
                    dependencies.Add(dependency);
                    Resolve<T>(dependency, args, dependencies);
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
                instance = (T)ctor.Invoke(null);
            }
            else
            {
                var ctorParamValues = new object[ctorParams.Length];
                for (var  i = 0; i < ctorParams.Length; i++)
                {
                    var ctorParam = ctorParams[i];
                    var ctorParamType = ctorParam.ParameterType;
                    var ctorParamValue = Resolve<T>(ctorParamType, args, dependencies);
                    ctorParamValues[i] = ctorParamValue;
                }
                instance = (T)ctor.Invoke(ctorParamValues);
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

                    var propValue = Resolve<T>(prop.PropertyType, args, dependencies);
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
                        var paramInstance = Resolve<T>(paramType, args, dependencies);
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