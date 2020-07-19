namespace SuperMaxim.IOC.Container
{
    public interface ITypeMap<in T>
    {
        ITypeMap<T> To<TM>(string key = null) where TM : T;

        ITypeMap<T> Singleton<TM>(string key = null) where TM : T;
        
        ITypeMap<T> Singleton<TM>(T instance, string key = null) where TM : T;
    }
}