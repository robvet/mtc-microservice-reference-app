using System;

namespace SharedUtilities.ExceptionProcessing
{
    public class ExceptionHandlingUtilties
    {
        /// <summary>
        ///     Enumerates exception collection fetching the original exception, which would be the real
        ///     error. The outer exceptions are wrappers which we are not concerned.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string TraverseException(Exception exception)
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

        public class JsonErrorResponse
        {
            public string Error { get; set; }
            public string Issue { get; set; }
            public string StackTrace { get; set; }
        }
    }
}