namespace Draftor.Domain.Models;

public class Person
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public virtual List<Transaction>? Transactions { get; set; }

    public virtual List<Group>? Groups { get; set; }
}