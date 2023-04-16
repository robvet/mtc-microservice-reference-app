using System.Net;
using System.Net.Http;

namespace RestCommunicator
{
    public class RestResponse<T>
    {
        public RestResponse(T data, string errorMessage, HttpStatusCode httpStatusCode)
        {
            Data = data;
            ErrorMessage = errorMessage;
            HttpStatusCode = httpStatusCode;
        }

        public T Data { get; }
        public string ErrorMessage { get; }
        public HttpStatusCode HttpStatusCode { get; }
    }
}