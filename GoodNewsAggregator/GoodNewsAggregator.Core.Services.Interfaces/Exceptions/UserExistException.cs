using System;

namespace GoodNewsAggregator.Core.Services.Interfaces.Exceptions
{
    public class UserExistException : Exception
    {
        public UserExistException(string message) : base(message)
        { }

        public UserExistException() : base()
        { }
    }
}