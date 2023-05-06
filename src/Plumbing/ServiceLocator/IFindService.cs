namespace ServiceLocator
{
    public interface IFindService
    {
        string GetServiceUri(ServiceEnum serviceName);
    }
}