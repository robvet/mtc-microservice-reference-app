namespace ServiceDiscovery
{
    public interface IServiceLocator
    {
        string GetServiceUri(ServiceEnum serviceName);
    }
}