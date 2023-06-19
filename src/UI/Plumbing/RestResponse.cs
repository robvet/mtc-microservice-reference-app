using System.Net.Http;

namespace MusicStore.Plumbing
{
    public class RestResponse<T> 
    {
        public RestResponse(HttpResponseMessage httpResponseMessage, T data, string errorMessage)
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