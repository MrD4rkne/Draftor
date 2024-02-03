using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Draftor.Models;

[Table("people")]
public class Person
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.CascadeInsert | CascadeOperation.CascadeRead | CascadeOperation.CascadeDelete)]
    public virtual List<Transaction> Transactions { get; set; }
}