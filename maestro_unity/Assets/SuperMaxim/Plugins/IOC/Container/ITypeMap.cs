using System;
using System.Diagnostics.CodeAnalysis;

namespace SuperMaxim.IOC.Container
{
    public interface ITypeMap<in T>
    {
        ITypeMap<T> To<TM>(string key = null) where TM : class, T;

        ITypeMap<T> Singleton<TM>(string key = null) where TM : class, T;

        ITypeMap<T> Singleton<TM>([DisallowNull] TM instance, string key = null) where TM : class, T;
    }

    internal interface ITypeMap : IDisposable
    {
        Type GetMappedType();
    }
}
