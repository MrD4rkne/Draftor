using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Draftor.Models;

[Table("transactions")]
public class Transaction
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    [ForeignKey(typeof(Person))]
    public int PersonId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public double Value { get; set; }

    public bool IsArchived { get; set; }

    public DateTime Date { get; set; }
}