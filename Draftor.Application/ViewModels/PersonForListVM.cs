namespace Draftor.Core.ViewModels;

public record PersonForListVM
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required bool Checked { get; set; }
}