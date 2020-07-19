namespace SuperMaxim.IOC.Container
{
    public interface ITypeMapResolver
    {
        T Instance<T>(string key = null, params object[] args);

        T Inject<T>(T instance, params object[] args);
    }
}