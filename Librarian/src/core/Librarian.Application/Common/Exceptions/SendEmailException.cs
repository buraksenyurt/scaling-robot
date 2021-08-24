using System;

namespace Librarian.Application.Common.Exceptions
{
    public class SendEmailException
        : Exception
    {
        public SendEmailException(string message)
            : base(message)
        {
        }
    }
}
