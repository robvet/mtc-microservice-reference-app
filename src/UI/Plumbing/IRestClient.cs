using System.Threading.Tasks;

namespace MusicStore.Plumbing
{
    public interface IRestClient
    {
        Task<RestResponse<TReturnMessage>> GetAsync<TReturnMessage>(string path)
            where TReturnMessage : class, new();

        Task<RestResponse<TReturnMessage>> PostAsync<TReturnMessage>(string path, object dataObject = null)
            where TReturnMessage : class, new();

        Task<RestResponse<TReturnMessage>> PutAsync<TReturnMessage>(string path, object dataObject = null)
            where TReturnMessage : class, new();

        Task<RestResponse<bool>> DeleteAsync(string path);
    }
}