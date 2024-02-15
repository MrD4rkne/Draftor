namespace Draftor.Domain.Models;

public class Transaction
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public double Value { get; set; }

    public bool IsArchived { get; set; }

    public DateTime Date { get; set; }

    public virtual Person? Person { get; set; }
}