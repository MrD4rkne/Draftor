namespace Draftor.Infrastructure;
public record DbConfigurationModel
{
    public required string DatabasePath { get; init; }
}