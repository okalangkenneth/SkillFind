using System;

namespace JobPosting.Application.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }
}
