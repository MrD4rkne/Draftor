namespace Draftor.Domain.Exceptions;

public class RepositoryWasNotInitedException : Exception
{
    private const string DEFAULT_MESSAGE = "Init method was not called. Repository does not have connection with database.";

    public RepositoryWasNotInitedException() : base(DEFAULT_MESSAGE)
    {
    }

    public RepositoryWasNotInitedException(string message) : base(message)
    {
    }

    public RepositoryWasNotInitedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}