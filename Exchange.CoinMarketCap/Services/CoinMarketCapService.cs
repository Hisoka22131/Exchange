using System.Net.Http.Json;
using Exchange.CoinMarketCap.Models;
using Exchange.CoinMarketCap.Models.Base;
using Exchange.CoinMarketCap.Options;
using Exchange.Common.Enums;
using Exchange.Domain.Enums;
using Exchange.Domain.Interfaces;

namespace Exchange.CoinMarketCap.Services;

internal sealed class CoinMarketCapService : ICreditsService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CoinMarketCapService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<int> CalculateCreditsAsync(CancellationToken ct)
    {
        const string endpoint = "/v1/key/info";
        
        using var httpClient = _httpClientFactory.CreateClient(CoinMarketCapOptions.HttpClientName);

        var response = await httpClient.GetAsync(endpoint, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<Status>(ct);

            throw new HttpRequestException(
                $"Не удалось получить кредиты: {response.StatusCode} {errorResponse?.ErrorMessage}");
        }

        var successResponse = await response.Content.ReadFromJsonAsync<GetCreditsResponse>(ct);
        
        return successResponse?.Data.Usage.CurrentMonth.CreditsLeft ?? 0;
    }
    
    public async Task<decimal> CalculateRateAsync(Currency source, Currency target, CancellationToken ct = default)
    {
        if (source == target)
            throw new ArgumentException("Валюты отправления и получения должны быть разные");

        var from = source.GetCurrency();
        var to = target.GetCurrency();
        
        var isFiat = from.IsFiat();
        
        // меняю местами, потому что, например, курс USD/USDT не существует в CoinMarketCap
        if (isFiat)
        {
            (from, to) = (to, from);
        }
        
        var endpoint = $"/v1/cryptocurrency/quotes/latest?symbol={from}&convert={to}";
        
        using var httpClient = _httpClientFactory.CreateClient(CoinMarketCapOptions.HttpClientName);

        var response = await httpClient.GetAsync(endpoint, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<Status>(ct);

            throw new HttpRequestException(
                $"Не удалось получить курс: {response.StatusCode} {errorResponse?.ErrorMessage}");
        }

        var successResponse = await response.Content.ReadFromJsonAsync<GetQuotesResponse>(ct);

        if (successResponse?.Data == null ||
            !successResponse.Data.TryGetValue(from.ToString(), out CurrencyData? data))
        {
            throw new InvalidOperationException($"Данные для валюты '{from}' не найдены.");
        }

        if (!data.Quote.TryGetValue(to.ToString(), out Quote? quote))
        {
            throw new InvalidOperationException($"Данные для котировки '{to}' не найдены.");
        }
        
        if (isFiat)
        {
            return 1 / quote.Price;
        }

        return quote.Price;
    }
}