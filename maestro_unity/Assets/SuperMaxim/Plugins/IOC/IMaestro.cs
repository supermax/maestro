namespace SuperMaxim.IOC
{
    public interface IMaestro
    {
        // TODO implement

        T Resolve<T>() where T : class;
    }
}