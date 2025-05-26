using Exchange.Common.Enums;
using Exchange.Core.Extensions;
using Exchange.Domain.Interfaces;
using MediatR;

namespace Exchange.Core.Mediator.Course.Query;

internal sealed class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, Domain.Entities.Course>
{
    private readonly IRateService _rateService;
    private readonly ICommissionService _commissionService;
    private readonly ILimitService _limitService;

    public GetCourseQueryHandler(
        IRateService rateService,
        ICommissionService commissionService,
        ILimitService limitService)
    {
        _rateService = rateService;
        _commissionService = commissionService;
        _limitService = limitService;
    }

    public async Task<Domain.Entities.Course> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        if (!request.FromCurrency.CanExchange(request.ToCurrency))
            throw new ArgumentException("Валюты отправления и получения должны быть разные");

        var rate = await _rateService.CalculateRateAsync(request.FromCurrency, request.ToCurrency, cancellationToken);

        rate = rate.RoundToDecimalPlaces(8);

        var (minExchangeFrom, maxExchangeFrom) =
            await _limitService.GetLimitAsync(request.FromCurrency, cancellationToken);

        minExchangeFrom = RoundValue(minExchangeFrom, request.FromCurrency);
        maxExchangeFrom = RoundValue(maxExchangeFrom, request.FromCurrency);

        decimal fromAmount;
        decimal toAmount;

        var tolerance = CalculateTolerance(request.ToCurrency);

        if (request.FromAmount == null && request.ToAmount == null)
        {
            // Оба значения null: берем минимальное значение
            fromAmount = RoundValue(minExchangeFrom, request.FromCurrency);
            toAmount = RoundValue(fromAmount * rate, request.ToCurrency);
        }
        else if (request.FromAmount != null && request.ToAmount == null)
        {
            // Указан только FromAmount
            fromAmount = RoundValue(request.FromAmount.Value, request.FromCurrency);
            toAmount = RoundValue(fromAmount * rate, request.ToCurrency);
        }
        else if (request.FromAmount == null && request.ToAmount != null)
        {
            // Указан только ToAmount
            toAmount = RoundValue(request.ToAmount.Value, request.ToCurrency);
            fromAmount = RoundValue(toAmount / rate, request.FromCurrency);
        }
        else
        {
            // Указаны оба значения
            fromAmount = RoundValue(request.FromAmount!.Value, request.FromCurrency);
            toAmount = RoundValue(request.ToAmount!.Value, request.ToCurrency);

            // Проверяем их соответствие курсу
            var calculatedToAmount = RoundValue(fromAmount * rate, request.ToCurrency);
            if (Math.Abs(calculatedToAmount - toAmount) > tolerance)
            {
                throw new ArgumentException("Валюты отправления и получения не соответствуют курсу");
            }
        }

        var minExchangeTo = RoundValue(minExchangeFrom * rate, request.ToCurrency);
        var maxExchangeTo = RoundValue(maxExchangeFrom * rate, request.ToCurrency);

        if (toAmount < minExchangeTo - tolerance || toAmount > maxExchangeTo + tolerance)
        {
            throw new ArgumentOutOfRangeException(nameof(toAmount),
                $"Валюта получения должна быть в пределах " +
                $"от {minExchangeTo} " +
                $"до {maxExchangeTo}");
        }

        var feeInfoInUsdt =
            await _commissionService.CalculateFeeInUsdtAsync(fromAmount, request.FromCurrency, cancellationToken);

        var feeInCurrentCurrency = await ConvertFromUsdtAsync(feeInfoInUsdt.Fee, request.ToCurrency, cancellationToken);

        return new Domain.Entities.Course
        {
            CourseFrom = 1,
            MaxAmountFrom = maxExchangeFrom,
            MinAmountFrom = minExchangeFrom,
            AmountFrom = fromAmount,

            CourseTo = rate,
            MaxAmountTo = maxExchangeTo,
            MinAmountTo = minExchangeTo,
            AmountTo = toAmount,

            FeeInUsdt = feeInfoInUsdt.Fee.RoundToDecimalPlaces(0),
            FeeInCurrency = feeInCurrentCurrency,
            FeePercent = feeInfoInUsdt.Percent
        };
    }

    private async Task<decimal> ConvertFromUsdtAsync(decimal amount, Currency currency, CancellationToken ct)
    {
        if (currency == Currency.USDT)
        {
            return amount;
        }

        var rate = await _rateService.CalculateRateAsync(Currency.USDT, currency, ct);

        return amount * rate;
    }

    private static decimal RoundValue(decimal value, Currency currency)
    {
        return Math.Round(value, currency.IsCrypto() && currency != Currency.USDT ? 7 : 2);
    }

    private static decimal CalculateTolerance(Currency currency)
    {
        return currency.IsCrypto() ? 0.0001m : 0.01m;
    }
}