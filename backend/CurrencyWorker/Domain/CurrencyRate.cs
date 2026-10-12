namespace CurrencyWorker.Domain;

public sealed record CurrencyRate(
    string Code,
    string CodeIn,
    string Name,
    decimal Bid,
    decimal Ask,
    decimal High,
    decimal Low,
    decimal PctChange,
    DateTimeOffset UpdatedAt)
{
    public string Pair => $"{Code}-{CodeIn}";
}