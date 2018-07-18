using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Data.Queue
{
    public class ExceptionQueueMessage<TException> where TException : Exception
    {
        public ExceptionQueueMessage() { }
        public ExceptionQueueMessage(TException exception, string message = null, string source = null, object data = null)
        {
            Exception = exception;
            Message = string.IsNullOrEmpty(message) ? message : exception.Message;
            Source = source;
            Data = data;
        }
        public string Message { get; set; }
        public string Source { get; set; }
        public object Data { get; set; }
        public TException Exception { get; set; }
    }
}
