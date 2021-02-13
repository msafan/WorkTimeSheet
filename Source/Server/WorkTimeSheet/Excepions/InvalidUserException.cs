using System;

namespace WorkTimeSheet.Excepions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException() { }
        public InvalidUserException(string? message) : base(message) { }
        public InvalidUserException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
