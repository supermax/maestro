using System;
using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Core.Objects;
using SuperMaxim.IOC.Attributes;
using SuperMaxim.IOC.Extensions;

namespace SuperMaxim.IOC
{
    public sealed class Maestro : IMaestro, IMaestroAdvanced
    {
        private static readonly Maestro DefaultInstance = new Maestro();

        public static IMaestro Default => DefaultInstance;

        public static IMaestroAdvanced Advanced => DefaultInstance;
        
        private Maestro()
        {
            
        }
        
        public T Resolve<T>() where T : class
        {
            // TODO
            var instance = Resolve(typeof(T), null) as T;
            return instance;
        }
        
        // TODO split into short methods
        private object Resolve(Type src, object[] args)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            if (src.IsAbstract)
            {
                throw new OperationCanceledException($"The {src} is an abstract class");
            }
            if (src.IsInterface)
            {
                throw new OperationCanceledException($"The {src} is an interface");
            }

            var mapAtt = src.GetCustomAttribute<TypeMapAttribute>();
            if (mapAtt != null && mapAtt.IsSingleton)
            {
                // TODO get from cache and return
                return null;
            }

            var depAtt = src.GetCustomAttribute<DependencyAttribute>();
            if (depAtt != null && !depAtt.Dependencies.IsNullOrEmpty())
            {
                foreach (var dependency in depAtt.Dependencies)
                {
                    // TODO check for circular dependency
                    Resolve(dependency, args);
                }
            }

            var ctor = src.GetDefaultConstructor();
            if (ctor == null)
            {
                throw new OperationCanceledException($"Cannot get default constructor for {src}");
            }

            object instance = null;
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
            if (!methods.IsNullOrEmpty())
            {
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
            }
            return instance;
        }
    }
}