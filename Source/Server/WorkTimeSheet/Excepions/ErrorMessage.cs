using System;

namespace WorkTimeSheet.Excepions
{
    public class ErrorMessage
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public Exception Exception { get; set; }
        public string Description { get; set; }
    }
}
