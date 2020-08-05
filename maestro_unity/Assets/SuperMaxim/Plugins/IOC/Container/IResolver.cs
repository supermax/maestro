using System;

namespace SuperMaxim.IOC.Container
{
    internal interface IResolver
    {
        T Resolve<T>(Type type, object[] args);
    }
}