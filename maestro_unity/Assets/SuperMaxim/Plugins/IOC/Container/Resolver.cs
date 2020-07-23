using System;
using System.Collections.Generic;
using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Attributes;
using SuperMaxim.IOC.Extensions;

namespace SuperMaxim.IOC.Container
{
    internal static class Resolver
    {
        // TODO split into short methods
        // TODO use args
        internal static object Resolve(Type src, object[] args, List<Type> dependencies)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            if (!src.IsClass)
            {
                throw new OperationCanceledException($"The {src} is not a class!");
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
                    if (dependencies.Contains(dependency))
                    {
                        // TODO write to log
                        continue;
                    }
                    dependencies.Add(dependency);
                    Resolve(dependency, args, dependencies);
                }
            }

            var ctor = src.GetDefaultConstructor();
            if (ctor == null)
            {
                throw new OperationCanceledException($"Cannot get constructor for {src}");
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
                    var ctorParamValue = Resolve(ctorParamType, args, dependencies);
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

                    var propValue = Resolve(prop.PropertyType, args, dependencies);
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
                        var paramInstance = Resolve(paramType, args, dependencies);
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