using System.Transactions;

namespace UnitSelection.Infrastructures.Test.Infrastructure;

public class DatabaseFixture : IDisposable
{
    private readonly TransactionScope _transactionScope;

    public DatabaseFixture()
    {
        _transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            TransactionScopeAsyncFlowOption.Enabled);
    }
    
    public void Dispose()
    {
        _transactionScope?.Dispose();
    }
}