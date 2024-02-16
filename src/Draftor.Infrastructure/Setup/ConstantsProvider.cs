using Draftor.Domain.Interfaces;

namespace Draftor.Infrastructure.Setup;

public class ConstantsProvider : IConstantsProvider
{
    private const string DatabaseFilename = "Draftor.db";

    private string GetDatabasePath()
    {
        var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(basePath, DatabaseFilename);
    }

    public object GetDatabaseConfigurationModel()
    {
        DbConfigurationModel dbConfigurationModel = new()
        {
            DatabasePath = GetDatabasePath()
        };
        return dbConfigurationModel;
    }
}