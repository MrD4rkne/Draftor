namespace Draftor.ViewModels;
public record TransactionVM
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required double Value { get; set; }

    public required bool IsArchived { get; set; }

    public required DateTime Date { get; set; }
}