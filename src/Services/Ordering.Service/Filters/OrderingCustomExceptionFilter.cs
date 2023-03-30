using System;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Ordering.API.Filters
{
    public class OrderingCustomExceptionFilter : IExceptionFilter {
        private const string ServiceName = "Ordering Service Error";
        private readonly ILogger _logger;
        
        public OrderingCustomExceptionFilter(ILogger<OrderingCustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var ServiceName = "Catalog Service Error";
            var source = context.Exception.Source;
            var message = $"{TraverseException(context.Exception)} occured in {source}";
            var stackTrace = context.Exception.StackTrace;

            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                "Exception throw in {ServiceName} : {message}", ServiceName, message);

            var jsonErrorResponse = new JsonErrorResponse
            {
                //Messages = new[] {context.Exception.Message}
                Messages = new[] {message},
                Source = source,
                StackTrace = stackTrace
            };

            // Fetch the exception
            var exceptionType = context.Exception.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                context.Result = new UnauthorizedResult();
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                context.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ServiceName;

                //message = "Unauthorized Access";
                //status = HttpStatusCode.Unauthorized;
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
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }

        /// <summary>
        ///     Enumerates exception collection fetching the original exception, which would be the real
        ///     error. The outer exceptions are wrappers which we are not concerned.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static string TraverseException(Exception exception)
        {
            var message = string.Empty;
            var innerException = exception;

            // Enumerate through exception stack to get to innermost exception
            do
            {
                message = string.IsNullOrEmpty(innerException.Message) ? string.Empty : innerException.Message;
                innerException = innerException.InnerException;
            } while (innerException != null);

            return message;
        }

        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            public string Source { get; set; }

            public string StackTrace { get; set; }

            public object DeveloperMeesage { get; set; }
        }
    }
}

