namespace SuperMaxim.IOC.Container
{
    public interface ITypeMap
    {
        ITypeMap To<T>(string key = null);

        ITypeMap Singleton<T>(string key = null);
        
        ITypeMap Singleton<T>(T instance, string key = null);
    }
}