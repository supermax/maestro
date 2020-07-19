namespace SuperMaxim.IOC.Container
{
    public interface ITypeMapReset<in T>
    {
        ITypeMapReset<T> From<TM>(string key = null) where TM : T;

        ITypeMapReset<T> Singleton(string key = null);
        
        ITypeMapReset<T> Singleton(T instance, string key = null);
    }
}