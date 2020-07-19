using System;
using SuperMaxim.IOC.Container;

namespace SuperMaxim.IOC
{
    public interface IMaestro : IDisposable
    {
        ITypeMapResolver<T> Get<T>();
        
        ITypeMap<T> Map<T>();
        
        ITypeMapReset<T> UnMap<T>();
        
        void Reset<T>();
    }
}