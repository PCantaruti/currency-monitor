using System.Globalization;
using System.Net.Http.Json;
using CurrencyWorker.Abstractions;
using CurrencyWorker.Domain;

namespace CurrencyWorker.Providers.AwesomeApi;

internal sealed class AwesomeApiClient(HttpClient http) : ICurrencyProvider
{
    public async Task<IReadOnlyList<CurrencyRate>> GetRatesAsync(
        IEnumerable<string> pairs, CancellationToken ct)
    {
        var url = $"json/last/{string.Join(',', pairs)}";
        var data = await http.GetFromJsonAsync<Dictionary<string, AwesomeApiQuote>>(url, ct)
                   ?? [];

        return data.Values.Select(Map).ToList();
    }

    private static CurrencyRate Map(AwesomeApiQuote q) => new(
        q.Code,
        q.CodeIn,
        q.Name,
        Dec(q.Bid), Dec(q.Ask), Dec(q.High), Dec(q.Low), Dec(q.PctChange),
        DateTimeOffset.FromUnixTimeSeconds(
            long.Parse(q.Timestamp, CultureInfo.InvariantCulture)));

    private static decimal Dec(string v) =>
        decimal.Parse(v, CultureInfo.InvariantCulture);
}