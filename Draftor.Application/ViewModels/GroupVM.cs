namespace Draftor.Core.ViewModels;

public record GroupVM
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public required int MembersCount { get; set; }
}