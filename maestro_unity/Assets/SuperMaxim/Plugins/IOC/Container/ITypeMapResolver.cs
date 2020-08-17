namespace SuperMaxim.IOC.Container
{
    public interface ITypeMapResolver<T> where T : class
    {
        T Instance(string key = null, params object[] args);

        T Inject(T instance, params object[] args);
    }
}