namespace Draftor.Exceptions;

public class GroupTitleOverlapException : Exception
{
    public GroupTitleOverlapException()
    {
    }

    public GroupTitleOverlapException(string message) : base(message)
    {
    }

    public GroupTitleOverlapException(string message, Exception innerException) : base(message, innerException)
    {
    }
}