using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Draftor.Domain.Exceptions;
public class BadConfigurationException : Exception
{
    public BadConfigurationException()
    {
    }

    public BadConfigurationException(string? message) : base(message)
    {
    }

    public BadConfigurationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected BadConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
