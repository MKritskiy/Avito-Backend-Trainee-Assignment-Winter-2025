using System.Transactions;

namespace Infrastucture.General;

internal static class Helpers
{
    public static string GenerateSalt()
        => Guid.NewGuid().ToString();

    public static TransactionScope CreateTransactionScope(int seconds = 60)
        => new TransactionScope(
            TransactionScopeOption.Required,
            TimeSpan.FromSeconds(seconds),
            TransactionScopeAsyncFlowOption.Enabled);
}
