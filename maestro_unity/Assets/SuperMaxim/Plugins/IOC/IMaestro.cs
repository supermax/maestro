using System;
using SuperMaxim.IOC.Container;

namespace SuperMaxim.IOC
{
    public interface IMaestro : IDisposable
    {
        ITypeMapResolver Get<T>();
        
        ITypeMap Map<T>();
        
        ITypeMapReset UnMap<T>();
        
        void Reset<T>();
    }
}