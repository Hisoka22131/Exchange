using Exchange.Common.Enums;
using Exchange.Domain.Interfaces;

namespace Exchange.Web.MetaData;

using Microsoft.AspNetCore.Http;

internal sealed class ExchangeMetaDataHttpHeader : IExchangeMetaData
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExchangeMetaDataHttpHeader(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Currency GetCurrencyFrom()
    {
        return GetCurrencyFromHeader("currencyFrom");
    }

    public Currency GetCurrencyTo()
    {
        return GetCurrencyFromHeader("currencyTo");
    }

    private Currency GetCurrencyFromHeader(string headerName)
    {
        var headers = _httpContextAccessor.HttpContext?.Request.Headers;

        if (headers != null && headers.TryGetValue(headerName, out var value))
        {
            if (Enum.TryParse<Currency>(value, true, out var currency))
            {
                return currency;
            }
        }

        throw new InvalidOperationException($"Заголовок {headerName} отсутствует или имеет неверный формат.");
    }
}
