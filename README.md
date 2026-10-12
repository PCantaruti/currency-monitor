# CurrencyWorker

A **.NET 10** worker that periodically fetches currency exchange rates from a free API (AwesomeAPI), stores the values, and exposes them to a React front-end.

## What the worker does

1. At a configured interval (default: 60s), it queries the exchange rate API.
2. It maps the response to the domain model (`CurrencyRate`).
3. It saves the latest rates in a store (in-memory first, database later).
4. It exposes the data through an HTTP API for the front-end to consume.

```
AwesomeAPI ──► CurrencySyncWorker ──► IRateStore ──► API (/api/rates) ──► React
```

## Technologies

| Layer                | Technology                                                                    |
| -------------------- | ----------------------------------------------------------------------------- |
| Runtime              | .NET 10                                                                       |
| Worker               | `BackgroundService` + `PeriodicTimer`                                         |
| HTTP                 | `IHttpClientFactory` (typed client)                                           |
| Resilience           | `Microsoft.Extensions.Http.Resilience` (retry, timeout, circuit breaker)      |
| Configuration        | Options pattern (`IOptions<T>`) with startup validation                       |
| Exchange rate API    | [AwesomeAPI](https://docs.awesomeapi.com.br/api-de-moedas) (free, no API key) |
| Persistence (future) | EF Core + SQLite/PostgreSQL                                                   |
| Real-time (future)   | SignalR                                                                       |
| Front-end            | React + TypeScript + Vite + TanStack Query                                    |

## Folder structure

```
CurrencyWorker/
├── Abstractions/        # Contracts (interfaces)
│   ├── ICurrencyProvider.cs
│   └── IRateStore.cs
├── Domain/
│   └── CurrencyRate.cs
├── Options/
│   └── CurrencyOptions.cs
├── Providers/
│   └── AwesomeApi/      # AwesomeAPI-specific implementation
│       ├── AwesomeApiClient.cs
│       └── AwesomeApiQuote.cs
├── Storage/
│   └── InMemoryRateStore.cs
├── Workers/
│   └── CurrencySyncWorker.cs
├── appsettings.json
└── Program.cs
```

The worker depends only on the interfaces in `Abstractions/`, so the exchange rate API and the storage can be swapped without changing the worker logic.

## Configuration

`appsettings.json`:

```json
{
  "Currency": {
    "BaseUrl": "https://economia.awesomeapi.com.br/",
    "IntervalSeconds": 60,
    "Pairs": ["USD-BRL", "EUR-BRL", "BTC-BRL"]
  }
}
```

## Running

```bash
dotnet restore
dotnet build
dotnet run
```

Expected output (every 60s):

```
info: CurrencyWorker.Workers.CurrencySyncWorker[0]
      USD-BRL: 5.xxxx (0.xx%)
```

## Phase checklist

- [x] **Phase 1:** Worker fetching the API
- [ ] **Phase 2:** Expose data to the front-end
- [ ] **Phase 3:** Persistence and history
- [ ] **Phase 4:** Front-end (React + TypeScript)
- [ ] **Phase 5:** Real-time and quality
