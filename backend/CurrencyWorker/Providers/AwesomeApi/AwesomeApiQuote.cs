using System.Text.Json.Serialization;

namespace CurrencyWorker.Providers.AwesomeApi;

internal sealed record AwesomeApiQuote(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("codein")] string CodeIn,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("high")] string High,
    [property: JsonPropertyName("low")] string Low,
    [property: JsonPropertyName("pctChange")] string PctChange,
    [property: JsonPropertyName("bid")] string Bid,
    [property: JsonPropertyName("ask")] string Ask,
    [property: JsonPropertyName("timestamp")] string Timestamp);