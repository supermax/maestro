using System;
using SuperMaxim.IOC.Container;

namespace SuperMaxim.IOC
{
    public interface IMaestro
    {
        ITypeMapResolver<T> Get<T>();
        
        ITypeMap<T> Map<T>();
        
        ITypeMapReset<T> UnMap<T>();
        
        void UnMapAll<T>();
    }
}