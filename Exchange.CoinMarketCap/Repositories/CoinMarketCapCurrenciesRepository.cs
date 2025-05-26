using System.Globalization;
using System.Net.Http.Json;
using Exchange.CoinMarketCap.Extensions;
using Exchange.CoinMarketCap.Models;
using Exchange.CoinMarketCap.Models.Apb;
using Exchange.CoinMarketCap.Models.Base;
using Exchange.CoinMarketCap.Options;
using Exchange.Common.Enums;
using Exchange.Core.Repositories;
using Exchange.Domain.Entities;
using Exchange.Domain.Enums;
using Exchange.Domain.Interfaces;
using CurrencyData = Exchange.Domain.Entities.CurrencyData;
using Quote = Exchange.Domain.Entities.Quote;

namespace Exchange.CoinMarketCap.Repositories;

internal sealed class CoinMarketCapCurrenciesRepository : ICurrenciesRepository
{
    private const decimal CurrentRupPmrUsdCourse = 16.4m;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICacheService _cacheService;

    public CoinMarketCapCurrenciesRepository(IHttpClientFactory httpClientFactory, ICacheService cacheService)
    {
        _httpClientFactory = httpClientFactory;
        _cacheService = cacheService;
    }

    public async Task<CurrencyInfo> GetAllCurrenciesAsync(CancellationToken ct = default)
    {
        const string endpoint = "/v1/cryptocurrency/quotes/latest?" +
                                "symbol=USDT,BTC,ETH,TRX" +
                                "&" +
                                "convert=USDT,BTC,ETH,TRX,USD,EUR,MDL,RUB";

        using var httpClient = _httpClientFactory.CreateClient(CoinMarketCapOptions.HttpClientName);

        var response = await httpClient.GetAsync(endpoint, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<Status>(ct);

            throw new HttpRequestException(
                $"Не удалось получить курс: {response.StatusCode} {errorResponse?.ErrorMessage}");
        }

        var successResponse = await response.Content.ReadFromJsonAsync<GetQuotesResponse>(ct);

        if (successResponse?.Data is null)
        {
            throw new ArgumentNullException("Ответ от CMC пустой!", nameof(successResponse));
        }

        var currencyInfo = new CurrencyInfo
        {
            Data = successResponse.Data.ToDictionary(
                kvp => kvp.Key,
                kvp => new CurrencyData
                {
                    Name = kvp.Value.Name,
                    Symbol = kvp.Value.Symbol,
                    Quote = kvp.Value.Quote.ToDictionary(
                        quoteKvp => quoteKvp.Key,
                        quoteKvp => new Quote
                        {
                            Price = quoteKvp.Value.Price,
                            IsGrow = quoteKvp.Value.PercentChange1H > 0
                        })
                })
        };

        return currencyInfo;
    }

    public async Task UpdateCurrenciesAsync(CancellationToken ct = default)
    {
        var currencyInfo = await GetAllCurrenciesAsync(ct);

        currencyInfo.Data["USDT"].Quote["USD"].Price = 1;

        // ApplyMarkupToAll(currencyInfo, "RUB", 3);

        var currencyInfoRubPmr = await GetRubPmrAsync(ct);
        var rate = currencyInfoRubPmr?.Data?[$"{Currency.RUP}"]?.Quote[$"{Currency.USD}"]?.Price ??
                   CurrentRupPmrUsdCourse;

        _cacheService.Merge<CurrencyInfo>(
            key: "allCurrencies",
            update: existingCurrencies =>
            {
                foreach (var (symbol, newCurrencyData) in currencyInfo.Data)
                {
                    if (!existingCurrencies.Data.TryAdd(symbol, newCurrencyData))
                    {
                        var existingCurrencyData = existingCurrencies.Data[symbol];

                        foreach (var (quoteSymbol, newQuote) in newCurrencyData.Quote)
                        {
                            if (!existingCurrencyData.Quote.TryAdd(quoteSymbol, newQuote))
                            {
                                existingCurrencyData.Quote[quoteSymbol] = newQuote;
                            }
                        }

                        existingCurrencies.Data[symbol] = existingCurrencyData;
                    }
                }

                existingCurrencies.AddRubPmrCurrency(Currency.RUP.ToString(), rate);

                return existingCurrencies;
            });
    }

    private static void ApplyMarkupToAll(CurrencyInfo currencyInfo, string quoteSymbol, decimal percentage)
    {
        foreach (var (_, currencyData) in currencyInfo.Data)
        {
            if (currencyData.Quote.TryGetValue(quoteSymbol, out var quote))
            {
                quote.Price *= 1 - percentage / 100;
            }
        }
    }

    private async Task<CurrencyInfo> GetRubPmrAsync(CancellationToken ct = default)
    {
        var endpoint = $"includes/histratesnew.php?type=all&date={DateTime.Now:dd.MM.yyyy}&json=1";

        using var httpClient = _httpClientFactory.CreateClient(ApbOptions.HttpClientName);

        string? rate = null;

        // Сделал небольшой хардкод, потому что АПБ может упасть и берем курс на конец 2024 года
        try
        {
            var response = await httpClient.GetAsync(endpoint, ct);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Не удалось получить курс: {response.StatusCode} {response.ReasonPhrase}");
            }

            var successResponse = await response.Content.ReadFromJsonAsync<ExchangeData>(ct);

            rate = successResponse?.Ib?.Rates?["1"]?.ValueSell ??
                   CurrentRupPmrUsdCourse.ToString(CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            rate ??= CurrentRupPmrUsdCourse.ToString(CultureInfo.InvariantCulture);
        }

        var currencyInfo = new CurrencyInfo
        {
            Data = new Dictionary<string, CurrencyData>
            {
                {
                    Currency.RUP.ToString(),
                    new CurrencyData
                    {
                        Name = "Рубль ПМР",
                        Symbol = Currency.RUP.ToString(),
                        Quote = new Dictionary<string, Quote>
                        {
                            {
                                Currency.USD.ToString(),
                                new Quote
                                {
                                    Price = Convert.ToDecimal(rate, CultureInfo.InvariantCulture) + 0.05m
                                }
                            }
                        }
                    }
                }
            }
        };

        return currencyInfo;
    }
}