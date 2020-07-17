using System;
using System.Data;
using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Attributes;
using SuperMaxim.IOC.Extensions;
using Object = UnityEngine.Object;

namespace SuperMaxim.IOC
{
    public class DIMaestro : IMaestro
    {
        public T Resolve<T>(string key = null, params object[] args)
        {
            // TODO
            return default;
        }
        
        // TODO split into short methods
        private object Resolve(Type src)
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
                    // check for circular dependency
                    Resolve(dependency);
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
                foreach (var ctorParam in ctorParams)
                {
                    // TODO init param
                }
                // TODO invoke ctor with params
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

                    var propValue = Resolve(prop.PropertyType);
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
                            var paramInstance = Resolve(paramType);
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