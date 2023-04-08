using System.Threading.Tasks;

namespace MusicStore.Helper
{
    public interface IRestClient
    {
        Task<UIRestResponse<TReturnMessage>> GetAsync<TReturnMessage>(string path)
            where TReturnMessage : class, new();

        Task<UIRestResponse<TReturnMessage>> PostAsync<TReturnMessage>(string path, object dataObject = null)
            where TReturnMessage : class, new();

        Task<UIRestResponse<TReturnMessage>> PutAsync<TReturnMessage>(string path, object dataObject = null)
            where TReturnMessage : class, new();

        Task<UIRestResponse<bool>> DeleteAsync(string path);
    }
}