using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class ExceptionUtilities
    {
        /// <summary>
        ///     Enumerates exception collection fetching the original exception, which would be the real
        ///     error. The outer exceptions are wrappers which we are not concerned.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string TraverseException(Exception exception)
        {
            var message = new StringBuilder();
            var innerException = exception;

            // Enumerate through exception stack to get to innermost exception
            do
            {
                message.Append(string.IsNullOrEmpty(innerException.Message) ? string.Empty : innerException.Message);
                innerException = innerException.InnerException;
            } while (innerException != null);

            return message.ToString();
        }

        public class JsonErrorResponse
        {
            public string Error { get; set; }
            public string Issue { get; set; }
            public string StackTrace { get; set; }
        }

    }
}
