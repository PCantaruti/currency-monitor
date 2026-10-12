using System.Collections.Concurrent;
using CurrencyWorker.Abstractions;
using CurrencyWorker.Domain;

namespace CurrencyWorker.Storage;

internal sealed class InMemoryRateStore : IRateStore
{
    private readonly ConcurrentDictionary<string, CurrencyRate> _rates = new();

    public Task SaveAsync(IEnumerable<CurrencyRate> rates, CancellationToken ct)
    {
        foreach (var r in rates) _rates[r.Pair] = r;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<CurrencyRate>> GetLatestAsync(CancellationToken ct) =>
        Task.FromResult<IReadOnlyList<CurrencyRate>>(
            _rates.Values.OrderBy(r => r.Pair).ToList());
}