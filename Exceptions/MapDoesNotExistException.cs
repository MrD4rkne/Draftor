namespace Draftor.Exceptions;

public class MapDoesNotExistException : Exception
{
    public MapDoesNotExistException()
    {
    }

    public MapDoesNotExistException(string message) : base(message)
    {
    }

    public MapDoesNotExistException(string message, Exception innerException) : base(message, innerException)
    {
    }
}