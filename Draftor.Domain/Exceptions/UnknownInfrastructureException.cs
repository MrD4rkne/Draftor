using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Draftor.Domain.Exceptions;
public class UnknownInfrastructureException : Exception
{
    public UnknownInfrastructureException()
    {
    }

    public UnknownInfrastructureException(string? message) : base(message)
    {
    }

    public UnknownInfrastructureException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
