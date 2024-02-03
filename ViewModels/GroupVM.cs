using Draftor.Models;

namespace Draftor.ViewModels;

public record GroupVM
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int MembersCount { get; set; }

    public GroupVM(Group group, int membersCount)
    {
        ArgumentNullException.ThrowIfNull(group, nameof(group));

        Id = group.Id;
        Title = group.Title;
        MembersCount = membersCount;
    }
}