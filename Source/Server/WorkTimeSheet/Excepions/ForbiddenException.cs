using System;

namespace WorkTimeSheet.Excepions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() { }
        public ForbiddenException(string? message) : base(message) { }
        public ForbiddenException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
