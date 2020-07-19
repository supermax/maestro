namespace SuperMaxim.IOC.Container
{
    public interface ITypeMapReset
    {
        ITypeMap From<T>(string key = null);

        ITypeMap Singleton<T>(string key = null);
        
        ITypeMap Singleton<T>(T instance, string key = null);
    }
}