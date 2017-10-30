using System;

namespace LibraryAPI.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base() { }
        public AlreadyExistsException(string msg) : base(msg) { }
    }
}
