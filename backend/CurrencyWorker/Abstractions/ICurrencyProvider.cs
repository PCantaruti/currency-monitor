using CurrencyWorker.Domain;

namespace CurrencyWorker.Abstractions;

public interface ICurrencyProvider
{
    Task<IReadOnlyList<CurrencyRate>> GetRatesAsync(
        IEnumerable<string> pairs, CancellationToken ct);
}