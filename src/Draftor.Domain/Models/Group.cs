namespace Draftor.Domain.Models;

public class Group
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public virtual List<Person>? Members { get; set; }
}