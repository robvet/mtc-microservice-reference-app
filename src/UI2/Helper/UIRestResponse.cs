using System.Net.Http;

namespace MusicStore.Helper
{
    public class UIRestResponse<T> 
    {
        public UIRestResponse(HttpResponseMessage httpResponseMessage, T data, string errorMessage)
        {
            HttpResponseMessage = httpResponseMessage;
            Data = data;
            ErrorMessage = errorMessage;
        }
        public HttpResponseMessage HttpResponseMessage { get; }

        public T Data { get; }

        public string ErrorMessage { get; }
    }
}