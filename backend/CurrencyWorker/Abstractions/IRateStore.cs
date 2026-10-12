using CurrencyWorker.Domain;

namespace CurrencyWorker.Abstractions;

public interface IRateStore
{
    Task SaveAsync(IEnumerable<CurrencyRate> rates, CancellationToken ct);
    Task<IReadOnlyList<CurrencyRate>> GetLatestAsync(CancellationToken ct);
}