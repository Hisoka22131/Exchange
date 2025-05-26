using Exchange.Common.Constants;
using Exchange.Common.Enums;
using Exchange.Core.Extensions;
using Exchange.Core.Records;
using Exchange.Core.Repositories;
using Exchange.Domain.Entities;
using Exchange.Domain.Enums;
using Exchange.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Exchange.Core.Mediator.Transactions.Commands.Init;

internal sealed class InitTransactionCommandHandler
    : IRequestHandler<InitTransactionCommand, InitTransactionCommandResponse>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Options.PhoneNumbers _options;

    public InitTransactionCommandHandler(IServiceProvider serviceProvider, IOptions<Options.PhoneNumbers> options)
    {
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    public async Task<InitTransactionCommandResponse> Handle(
        InitTransactionCommand req,
        CancellationToken cancellationToken
    )
    {
        if (!req.CurrencyFrom.CanExchange(req.CurrencyTo))
        {
            throw new ArgumentException("Валюты отправления и получения должны быть разные");
        }

        using var scope = _serviceProvider.CreateScope();
        {
            if (req.CurrencyFrom.IsCrypto() && req.CurrencyTo.IsFiat())
            {
                return await InitCryptoToFiatTransactionAsync(req, cancellationToken);
            }

            return await InitFiatToCryptoTransactionAsync(req, cancellationToken);
        }
    }

    private async Task<InitTransactionCommandResponse> InitCryptoToFiatTransactionAsync(
        InitTransactionCommand args,
        CancellationToken cancellationToken
    )
    {
        if (!args.CurrencyFrom.IsCrypto())
        {
            throw new ArgumentException("Invalid currency");
        }

        var (fromAmount, toAmount, feeInfoInUsdt, feeInCurrency, amountToInUsdt) =
            await ValidateAndReCalculateAsync(args, cancellationToken);

        var transactionId = Guid.NewGuid();

        var cryptoNetwork = DigitalAssets.GetNetworkByCurrency(args.CurrencyFrom, args.NetworkCryptoCode);

        if (cryptoNetwork is null)
        {
            throw new ArgumentNullException(nameof(cryptoNetwork));
        }

        var fiatNetwork = DigitalAssets.GetNetworkByCurrency(args.CurrencyTo, args.NetworkFiatCode);

        if (fiatNetwork is null)
        {
            throw new ArgumentNullException(nameof(fiatNetwork));
        }

        var walletAddress = cryptoNetwork.WalletAddress!;

        var transactionInfo = new TransactionCacheInfo(
            TransactionId: transactionId,
            CurrencyFrom: args.CurrencyFrom,
            CurrencyTo: args.CurrencyTo,
            AmountFrom: fromAmount,
            AmountTo: toAmount,
            CommissionInUsdt: feeInfoInUsdt,
            City: args.City,
            PhoneNumber: args.PhoneNumber,
            CryptoNetworkCode: cryptoNetwork.Code,
            CryptoNetworkName: cryptoNetwork.Name,
            FiatNetworkCode: fiatNetwork.Code,
            FiatNetworkName: fiatNetwork.Name,
            WalletAddressAdmin: walletAddress,
            WalletAddressUser: args.PhoneNumber,
            TelegramUserId: args.UserId,
            TelegramUserName: args.UserName,
            AmountToInUsdt: amountToInUsdt);

        await SaveTransactionAndUserAsync(transactionInfo, cancellationToken);

        return new InitTransactionCommandResponse(
            FromCrypto: true,
            TransactionId: transactionId,
            Address: walletAddress,
            AmountFrom: fromAmount,
            AmountTo: toAmount,
            FeeInUsdt: feeInfoInUsdt,
            FeeInCurrency: feeInCurrency,
            PhoneNumber: null
        );
    }

    private async Task<InitTransactionCommandResponse> InitFiatToCryptoTransactionAsync(
        InitTransactionCommand args,
        CancellationToken cancellationToken
    )
    {
        if (args.CurrencyFrom.IsCrypto())
        {
            throw new ArgumentException("Invalid currency");
        }

        var (fromAmount, toAmount, feeInfoInUsdt, feeInCurrency, amountToInUsdt) =
            await ValidateAndReCalculateAsync(args, cancellationToken);

        var transactionId = Guid.NewGuid();

        var cryptoNetwork = DigitalAssets.GetNetworkByCurrency(args.CurrencyTo, args.NetworkCryptoCode);

        if (cryptoNetwork is null)
        {
            throw new ArgumentNullException(nameof(cryptoNetwork));
        }

        var fiatNetwork = DigitalAssets.GetNetworkByCurrency(args.CurrencyFrom, args.NetworkFiatCode);

        if (fiatNetwork is null)
        {
            throw new ArgumentNullException(nameof(fiatNetwork));
        }

        var walletAddressUser = args.WalletAddressUser!;

        if (string.IsNullOrWhiteSpace(walletAddressUser))
        {
            throw new ArgumentNullException(nameof(walletAddressUser), "Wallet address user is required");
        }

        var walletAddressAdmin = fiatNetwork.Code.IsCash() ? fiatNetwork.Name : GetPhoneNumberByCity(args.City);

        var transactionInfo = new TransactionCacheInfo(
            TransactionId: transactionId,
            CurrencyFrom: args.CurrencyFrom,
            CurrencyTo: args.CurrencyTo,
            AmountFrom: fromAmount,
            AmountTo: toAmount,
            CommissionInUsdt: feeInfoInUsdt,
            City: args.City,
            PhoneNumber: args.PhoneNumber,
            CryptoNetworkCode: cryptoNetwork.Code,
            CryptoNetworkName: cryptoNetwork.Name,
            FiatNetworkCode: fiatNetwork.Code,
            FiatNetworkName: fiatNetwork.Name,
            WalletAddressAdmin: walletAddressAdmin,
            WalletAddressUser: walletAddressUser,
            TelegramUserId: args.UserId,
            TelegramUserName: args.UserName,
            AmountToInUsdt: amountToInUsdt);

        await SaveTransactionAndUserAsync(transactionInfo, cancellationToken);

        return new InitTransactionCommandResponse(
            FromCrypto: false,
            TransactionId: transactionId,
            AmountFrom: fromAmount,
            AmountTo: toAmount,
            FeeInUsdt: feeInfoInUsdt,
            FeeInCurrency: feeInCurrency,
            PhoneNumber: walletAddressAdmin);
    }

    private async Task SaveTransactionAndUserAsync(
        TransactionCacheInfo transactionInfo,
        CancellationToken cancellationToken = default
    )
    {
        using var scope = _serviceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var user = await unitOfWork.Users.GetByTelegramCredentialsAsync(
            transactionInfo.TelegramUserName,
            cancellationToken);

        var isNewUser = user is null;

        if (isNewUser)
        {
            user = new UserEntity(
                telegramUserId: transactionInfo.TelegramUserId,
                telegramUserName: transactionInfo.TelegramUserName);
        }

        var transactionEntity = new TransactionEntity
        {
            Id = transactionInfo.TransactionId,
            CurrencyFrom = transactionInfo.CurrencyFrom,
            CurrencyTo = transactionInfo.CurrencyTo,
            AmountFrom = transactionInfo.AmountFrom,
            AmountTo = transactionInfo.AmountTo,
            Commission = transactionInfo.CommissionInUsdt,
            City = transactionInfo.City,
            PhoneNumberUser = transactionInfo.PhoneNumber,
            CryptoNetworkCode = transactionInfo.CryptoNetworkCode,
            CryptoNetworkName = transactionInfo.CryptoNetworkName,
            FiatNetworkCode = transactionInfo.FiatNetworkCode,
            FiatNetworkName = transactionInfo.FiatNetworkName,
            WalletAddressUser = transactionInfo.WalletAddressUser,
            WalletAddressAdmin = transactionInfo.WalletAddressAdmin,
            State = TransactionState.Init,
            User = user!,
            UserId = user!.Id,
            AmountToInUsdt = transactionInfo.AmountToInUsdt
        };

        if (isNewUser)
        {
            await unitOfWork.Users.AddAsync(user, cancellationToken);
        }
        else
        {
            unitOfWork.Users.Update(user);
        }

        await unitOfWork.Transactions.AddAsync(transactionEntity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<(decimal fromAmount, decimal toAmount, decimal feeInUsdt, decimal feeInCurrency, decimal
            amountToInUsdt)>
        ValidateAndReCalculateAsync(InitTransactionCommand args,
            CancellationToken cancellationToken)
    {
        var rateService = _serviceProvider.GetRequiredService<IRateService>();
        var limitService = _serviceProvider.GetRequiredService<ILimitService>();
        var commissionService = _serviceProvider.GetRequiredService<ICommissionService>();

        var tolerance = CalculateTolerance(args.CurrencyTo);

        // 1. Проверка валют
        if (!args.CurrencyFrom.CanExchange(args.CurrencyTo))
        {
            throw new ArgumentException("Валюты отправления и получения должны быть разные");
        }

        // 2. Получение курса обмена
        var rate = await rateService.CalculateRateAsync(args.CurrencyFrom, args.CurrencyTo, cancellationToken);
        rate = rate.RoundToDecimalPlaces(8);

        // 3. Получение лимитов
        var (minExchangeFrom, maxExchangeFrom) =
            await limitService.GetLimitAsync(args.CurrencyFrom, cancellationToken);

        minExchangeFrom = RoundValue(minExchangeFrom, args.CurrencyFrom);
        maxExchangeFrom = RoundValue(maxExchangeFrom, args.CurrencyFrom);

        // 4. Пересчёт и валидация сумм
        decimal fromAmount = RoundValue(args.AmountFrom, args.CurrencyFrom);
        decimal toAmount = RoundValue(args.AmountTo, args.CurrencyTo);

        var calculatedToAmount = RoundValue(fromAmount * rate, args.CurrencyTo);
        if (Math.Abs(calculatedToAmount - toAmount) > tolerance)
        {
            throw new ArgumentException("Сумма получения не соответствует рассчитанной по курсу");
        }

        if (fromAmount < minExchangeFrom - tolerance || fromAmount > maxExchangeFrom + tolerance)
        {
            throw new ArgumentOutOfRangeException(nameof(fromAmount),
                $"Сумма отправления должна быть в пределах от {minExchangeFrom} до {maxExchangeFrom}");
        }

        // 5. Расчёт комиссии
        var feeInfoInUsdt =
            await commissionService.CalculateFeeInUsdtAsync(fromAmount, args.CurrencyFrom, cancellationToken);

        var feeInUsdt = feeInfoInUsdt.Fee.RoundToDecimalPlaces(0);

        if (Math.Abs(feeInUsdt - args.FeeInUsdt) > tolerance)
        {
            throw new ArgumentException("Комиссия не соответствует рассчитанной");
        }

        var feeInCurrentCurrency =
            await ConvertFromUsdtAsync(rateService, feeInfoInUsdt.Fee, args.CurrencyTo, cancellationToken);

        var amountToInUsdt = await ConvertToUsdtAsync(rateService, toAmount, args.CurrencyTo, cancellationToken);

        return (
            fromAmount: fromAmount,
            toAmount: RoundValue(toAmount - feeInCurrentCurrency, args.CurrencyTo),
            feeInUsdt,
            feeInCurrency: RoundValue(feeInCurrentCurrency, args.CurrencyTo),
            amountToInUsdt: RoundValue(amountToInUsdt, Currency.USDT));
    }

    private static async Task<decimal> ConvertToUsdtAsync(
        IRateService rateService,
        decimal amount,
        Currency currency,
        CancellationToken ct)
    {
        if (currency == Currency.USDT)
        {
            return amount;
        }

        var rate = await rateService.CalculateRateAsync(currency.GetCurrency(), Currency.USDT, ct);

        return amount * rate;
    }
    
    private static async Task<decimal> ConvertFromUsdtAsync(
        IRateService rateService,
        decimal amount,
        Currency currency,
        CancellationToken ct)
    {
        if (currency == Currency.USDT)
        {
            return amount;
        }

        var rate = await rateService.CalculateRateAsync(Currency.USDT, currency, ct);

        return amount * rate;
    }


    private static decimal RoundValue(decimal value, Currency currency)
    {
        return Math.Round(value, currency.IsCrypto() && currency != Currency.USDT ? 7 : 2);
    }

    private string GetPhoneNumberByCity(string city)
    {
        return city switch
        {
            "Тирасполь" or "Бендеры" or "Дубоссары" or "Григориополь" or "Рыбница" or "Каменка"
                => _options.Values["PMR"],
            "Кишинёв" => _options.Values["MD"],
            "Москва" => _options.Values["RU"],
            _ => throw new ArgumentOutOfRangeException(nameof(city), city, null)
        };
    }

    private static decimal CalculateTolerance(Currency currency)
    {
        return currency.IsCrypto() ? 0.0001m : 0.01m;
    }
}