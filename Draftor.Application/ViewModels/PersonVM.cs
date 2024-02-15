namespace Draftor.Core.ViewModels;
public record PersonVM
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }
}