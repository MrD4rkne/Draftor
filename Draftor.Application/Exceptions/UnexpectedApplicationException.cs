using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Draftor.Core.Exceptions;
public class UnexpectedApplicationException : Exception
{
    public UnexpectedApplicationException()
    {
    }

    public UnexpectedApplicationException(string? message) : base(message)
    {
    }

    public UnexpectedApplicationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
