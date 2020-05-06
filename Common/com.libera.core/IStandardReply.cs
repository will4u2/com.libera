using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.libera.core
{
    /// <summary>
    /// Wrapper for any service level method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStandardReply<T>
    {
        /// <summary>
        /// Indicates if request was successful
        /// </summary>
        bool Success { get; set; }
        /// <summary>
        /// Contains the data requested
        /// </summary>
        T Response { get; set; }
        /// <summary>
        /// IEnumerable string list of messages returned from method
        /// </summary>
        List<string> Messages { get; set; }
        /// <summary>
        /// IEnumerable Exception list of any exceptions thrown within the service layers
        /// </summary>
        List<Exception> Exceptions { get; set; }
        /// <summary>
        /// Adds the current exception to reply
        /// </summary>
        /// <param name="exc">the current exception being thrown</param>
        /// <param name="logData">generic object to be included in the log</param>
        /// <param name="logger">an instance of the currently used log</param>
        /// <param name="methodName">name of method where the error occured</param>
        /// <param name="forward">indicates if exception should be thrown</param>
        void ProcessException(Exception exc, object logData, ILogger logger, string methodName, bool forward = false);
        /// <summary>
        /// Clears all messages from reply
        /// </summary>
        void ClearMessages();
        /// <summary>
        /// Clears all exceptions from reply
        /// </summary>
        void ClearExceptions();
    }
}
