using System;

namespace WorkTimeSheet.Exceptions
{
    public class ErrorMessage
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public Exception Exception { get; set; }
        public string Description { get; set; }
    }
}
