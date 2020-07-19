namespace SuperMaxim.IOC.Container
{
    public interface ITypeMapResolver<T>
    {
        T Instance(string key = null, params object[] args);

        T Inject(T instance, params object[] args);
    }
}