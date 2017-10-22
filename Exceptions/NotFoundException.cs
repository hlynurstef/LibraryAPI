using System;

namespace LibraryAPI.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string msg) : base(msg) { }
    }
}
