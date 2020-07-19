using System;
using System.Collections.Generic;
using System.Reflection;
using SuperMaxim.Core.Extensions;
using SuperMaxim.IOC.Attributes;
using SuperMaxim.IOC.Container;
using SuperMaxim.IOC.Extensions;

namespace SuperMaxim.IOC
{
    public sealed class Maestro : IMaestro
    {
        private static readonly Maestro DefaultInstance = new Maestro();

        public static IMaestro Default => DefaultInstance;

        private Maestro()
        {
        }
        
        public ITypeMapResolver Get<T>()
        {
            throw new NotImplementedException();
        }

        public ITypeMap Map<T>()
        {
            throw new NotImplementedException();
        }

        public ITypeMapReset UnMap<T>()
        {
            throw new NotImplementedException();
        }

        public void Reset<T>()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}