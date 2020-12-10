using System;

namespace WorkTimeSheet.Exceptions
{
    public class WebApiException : Exception
    {
        public WebApiException() : base() { }

        public WebApiException(string message) : base(message) { }

        public WebApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}
