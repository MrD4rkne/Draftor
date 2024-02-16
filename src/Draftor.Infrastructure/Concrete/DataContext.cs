using Draftor.Domain.Exceptions;
using Draftor.Domain.Interfaces;
using Draftor.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Draftor.Infrastructure.Concrete;

public class DataContext : DbContext
{
    private readonly string _databasePath;

    public DbSet<Person> People { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DataContext(IConstantsProvider constantsProvider)
    {
        if (constantsProvider.GetDatabaseConfigurationModel() is not DbConfigurationModel dbConfigurationModel)
            throw new BadConfigurationException();
        _databasePath = dbConfigurationModel.DatabasePath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_databasePath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasMany(person => person.Transactions)
            .WithOne(transaction => transaction.Person)
            .HasForeignKey(t => t.PersonId);
        modelBuilder.Entity<Group>()
            .HasMany(group => group.Members)
            .WithMany(person => person.Groups)
            .UsingEntity<Membership>(
            memb => memb.HasOne(m => m.Person)
                .WithMany()
                .HasForeignKey(e => e.PersonId),
            memb => memb.HasOne(m => m.Group)
                .WithMany()
                .HasForeignKey(e => e.GroupId)
            );
    }
}