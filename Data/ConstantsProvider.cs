using Draftor.Abstract;

namespace Draftor.Data;

public class ConstantsProvider : IConstantsProvider
{
    private const string DatabaseFilename = "Draftor.db";

    private const SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public string GetDatabasePath()
    {
        var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(basePath, DatabaseFilename);
    }

    public SQLite.SQLiteOpenFlags GetDatabaseFlags()
    {
        return Flags;
    }
}