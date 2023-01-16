using UnitSelection.Persistence.EF;
using Xunit;

namespace UnitSelection.Infrastructures.Test.Infrastructure;

[Collection(nameof(ConfigurationFixture))]
public class EFDataContextDatabaseFixture : DatabaseFixture
{
    readonly ConfigurationFixture _configuration;

    public EFDataContextDatabaseFixture(ConfigurationFixture configuration)
    {
        _configuration = configuration;
    }

    public EFDataContext CreateDataContext()
    {
        return new EFDataContext(_configuration.Value.DbConnectionString);
    }
}