using System;

namespace WorkTimeSheet.Excepions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException() { }
        public DataNotFoundException(string? message) : base(message) { }
        public DataNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
