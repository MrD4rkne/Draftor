namespace Draftor.Abstract;

public interface IConstantsProvider
{
    string GetDatabasePath();

    SQLite.SQLiteOpenFlags GetDatabaseFlags();
}