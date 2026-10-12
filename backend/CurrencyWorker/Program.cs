using CurrencyWorker.Abstractions;
using CurrencyWorker.Options;
using CurrencyWorker.Providers.AwesomeApi;
using CurrencyWorker.Storage;
using CurrencyWorker.Workers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddOptions<CurrencyOptions>()
    .BindConfiguration("Currency")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddHttpClient<ICurrencyProvider, AwesomeApiClient>((sp, client) =>
    {
        var opt = sp.GetRequiredService<IOptions<CurrencyOptions>>().Value;
        client.BaseAddress = new Uri(opt.BaseUrl);
    })
    .AddStandardResilienceHandler();

builder.Services.AddSingleton<IRateStore, InMemoryRateStore>();
builder.Services.AddHostedService<CurrencySyncWorker>();

await builder.Build().RunAsync();