using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Draftor.Models;

[Table("groups")]
public class Group
{
    [AutoIncrement]
    [PrimaryKey]
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead | CascadeOperation.CascadeDelete)]
    public virtual List<Person> Members { get; set; }
}