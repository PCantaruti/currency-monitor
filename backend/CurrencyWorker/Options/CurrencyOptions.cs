using System.ComponentModel.DataAnnotations;

namespace CurrencyWorker.Options;

public sealed class CurrencyOptions
{
    [Required, Url]
    public string BaseUrl { get; init; } = string.Empty;

    [Range(10, 3600)]
    public int IntervalSeconds { get; init; } = 60;

    [MinLength(1)]
    public string[] Pairs { get; init; } = [];
}