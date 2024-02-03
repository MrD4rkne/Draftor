namespace Draftor.Exceptions;

public class MappingFunctionException : Exception

{
    public MappingFunctionException()
    {
    }

    public MappingFunctionException(string message) : base(message)
    {
    }

    public MappingFunctionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}