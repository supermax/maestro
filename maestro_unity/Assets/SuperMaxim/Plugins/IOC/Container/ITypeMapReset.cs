namespace SuperMaxim.IOC.Container
{
    public interface ITypeMapReset<in T>
    {
        ITypeMapReset<T> From<TM>(string key = null) where TM : class, T;

        ITypeMapReset<T> From<TM>(TM instance, string key = null) where TM : class, T;
    }
}