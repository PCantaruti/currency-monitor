using CurrencyWorker.Abstractions;
using CurrencyWorker.Options;
using Microsoft.Extensions.Options;

namespace CurrencyWorker.Workers;

public sealed class CurrencySyncWorker(
    ICurrencyProvider provider,
    IRateStore store,
    IOptions<CurrencyOptions> options,
    ILogger<CurrencySyncWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var opt = options.Value;
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(opt.IntervalSeconds));

        do
        {
            try
            {
                var rates = await provider.GetRatesAsync(opt.Pairs, stoppingToken);
                await store.SaveAsync(rates, stoppingToken);

                foreach (var r in rates)
                    logger.LogInformation("{Pair}: {Bid} ({Pct}%)", r.Pair, r.Bid, r.PctChange);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Falha ao sincronizar cotações");
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}