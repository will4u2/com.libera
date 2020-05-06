using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.libera.core
{
    public partial class StandardReply<T> : IStandardReply<T>
    {
        private StandardReply() { }
        public static IStandardReply<T> CreateInstance(bool successDefault = true)
        {
            StandardReply<T> reply = new StandardReply<T>
            {
                Success = successDefault,
                Messages = new List<string>(),
                Exceptions = new List<Exception>()
            };
            return reply;
        }
        public static IStandardReply<T> CreateInstance(string firstMessage, bool successDefault = true)
        {
            StandardReply<T> reply = new StandardReply<T>
            {
                Success = successDefault,
                Messages = new List<string> { firstMessage },
                Exceptions = new List<Exception>()
            };
            return reply;
        }

        public void ProcessException(Exception exc, object logData, ILogger logger, string methodName, bool forward = false)
        {
            logger.LogError(exc, "{0} failed.  Data used: {1} ", methodName, JsonConvert.SerializeObject(logData));
            Success = false;
            Exceptions.Add(exc);
            Messages.Add(exc.Message.ToString());
            foreach (string m in exc.GetInnerExceptions())
            {
                Messages.Add(m);
            }
            if (forward)
            {
                throw exc;
            }
        }
        public void ClearMessages()
        {
            Messages.Clear();
        }

        public void ClearExceptions()
        {
            Exceptions.Clear();
        }

        public bool Success { get; set; }
        public T Response { get; set; }
        public List<string> Messages { get; set; }
        public List<Exception> Exceptions { get; set; }
    }
}
