using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Draftor.Models;

[Table("memberships")]
public class Membership
{
    [ForeignKey(typeof(Person))]
    public int PersonId { get; set; }

    [ForeignKey(typeof(Group))]
    public int GroupId { get; set; }
}