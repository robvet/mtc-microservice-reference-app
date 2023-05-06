using System;
using System.Net;
using System.Threading.Tasks;
using ServiceLocator;

namespace RestCommunicator
{
    public interface IRestClient
    {
        Task<RestResponse<TReturnMessage>> GetAsync<TReturnMessage>(ServiceEnum serviceName, string path, string correlationToken)
            where TReturnMessage : class, new();

        Task<RestResponse<TReturnMessage>> PostAsync<TReturnMessage>(ServiceEnum serviceName, string path, string correlationToken, object dataObject = null)
            where TReturnMessage : class, new();

        Task<RestResponse<TReturnMessage>> PutAsync<TReturnMessage>(ServiceEnum serviceName, string path, string correlationToken, object dataObject = null)
            where TReturnMessage : class, new();

        Task<bool> DeleteAsync(ServiceEnum serviceName, string path, string correlationToken = "111111");
    }
}