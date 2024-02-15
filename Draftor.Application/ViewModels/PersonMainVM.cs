namespace Draftor.Core.ViewModels;

public record PersonMainVM
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required double Total { get; set; }

    public bool IsLeft { get; set; }
}