using System;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Catalog.API.Filters
{
    public class CatalogCustomExceptionFilter : IExceptionFilter
    {
        private const string ServiceName = "Catalog Service Error";
        private readonly ILogger _logger;

        public CatalogCustomExceptionFilter(ILogger<CatalogCustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        //https://janaks.com.np/using-exceptionfilter-for-exception-handling-in-aspnet-core-web-api/

        public void OnException(ExceptionContext context)
        {
            var source = context.Exception.Source;
            var path = context.HttpContext.Request.Path;
            // Capture root exception and all inner exceptions
            var message = $"{ExceptionUtilities.TraverseException(context.Exception)} occured in {source}";
            var stackTrace = context.Exception.StackTrace;

            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                "Exception throw in {ServiceName} : {message}", ServiceName, message);

            var jsonErrorResponse = new ExceptionUtilities.JsonErrorResponse
            {
                Error = $"Exception thrown in {source} at {path}",
                Issue = message,
                StackTrace = stackTrace
            };

            // Fetch the exception type
            var exceptionType = context.Exception.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                context.Result = new UnauthorizedResult();
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                context.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ServiceName;
            }
            else if (exceptionType == typeof(ForbidResult))
            {
                context.Result = new ForbidResult(message);
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                context.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ServiceName;
            }
            else
            {
                context.Result = new ObjectResult(jsonErrorResponse);
                context.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = message;
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }

        ///// <summary>
        /////     Enumerates exception collection fetching the original exception, which would be the real
        /////     error. The outer exceptions are wrappers which we are not concerned.
        ///// </summary>
        ///// <param name="exception"></param>
        ///// <returns></returns>
        //private static string TraverseException(Exception exception)
        //{
        //    var message = new StringBuilder();
        //    var innerException = exception;

        //    // Enumerate through exception stack to get to innermost exception
        //    do
        //    {
        //        message.Append(string.IsNullOrEmpty(innerException.Message) ? string.Empty : innerException.Message);
        //        innerException = innerException.InnerException;
        //    } while (innerException != null);

        //    return message.ToString();
        //}

        //private class JsonErrorResponse
        //{
        //    public string Error { get; set; }
        //    public string Issue { get; set; }
        //    public string StackTrace { get; set; }
        //}
    }
}