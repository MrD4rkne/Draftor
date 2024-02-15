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