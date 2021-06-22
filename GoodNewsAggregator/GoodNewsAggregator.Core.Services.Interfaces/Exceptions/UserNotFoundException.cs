using System;

namespace GoodNewsAggregator.Core.Services.Interfaces.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        { }

        public UserNotFoundException() : base()
        { }
    }
}