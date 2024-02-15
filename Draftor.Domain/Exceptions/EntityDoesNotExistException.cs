namespace Draftor.Domain.Exceptions;

public class EntityDoesNotExistException : Exception
{
    private const string DEFAULT_MESSAGE = "Entity with provided info does not exist.";

    public EntityDoesNotExistException() : base(DEFAULT_MESSAGE)
    {
    }

    public EntityDoesNotExistException(string message) : base(message)
    {
    }

    public EntityDoesNotExistException(string message, Exception innerException) : base(message, innerException)
    {
    }
}