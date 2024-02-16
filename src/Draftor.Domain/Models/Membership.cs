namespace Draftor.Domain.Models;

public class Membership
{
    public int PersonId { get; set; }

    public int GroupId { get; set; }

    public Person? Person { get; set; }

    public Group? Group { get; set; }
}