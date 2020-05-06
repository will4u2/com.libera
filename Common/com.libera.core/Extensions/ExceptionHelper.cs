using System;
using System.Collections.Generic;
using System.Linq;

namespace com.libera.core
{
    public static class ExceptionHelper
    {
        public static IEnumerable<string> GetInnerExceptions(this Exception exception)
        {
            IEnumerable<string> messages = exception
            .GetAllExceptions()
            .Where(e => !String.IsNullOrWhiteSpace(e.Message))
            .Select(e => e.Message.Trim());
            return messages;
        }

        private static IEnumerable<Exception> GetAllExceptions(this Exception exception)
        {
            yield return exception;

            if (exception is AggregateException aggrEx)
            {
                foreach (Exception innerEx in aggrEx.InnerExceptions.SelectMany(e => e.GetAllExceptions()))
                {
                    yield return innerEx;
                }
            }
            else if (exception.InnerException != null)
            {
                foreach (Exception innerEx in exception.InnerException.GetAllExceptions())
                {
                    yield return innerEx;
                }
            }
        }
    }
}
