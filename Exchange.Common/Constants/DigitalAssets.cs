using Exchange.Common.Enums;
using Exchange.Common.Models;

namespace Exchange.Common.Constants;

public static class DigitalAssets
{
    public static readonly List<DigitalAsset> Currencies =
    [
        new DigitalAsset
        {
            Value = Currency.USDT,
            Icon = "💲",
            Label = "USDT",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.BSC, Name = "BNB Smart Chain (BEP20)",
                    WalletAddress = "0xffb4fc4b54415fec990186733c656935496c8b79"
                },

                new Network
                {
                    Code = NetworkCode.ETH, Name = "Ethereum (ERC20)",
                    WalletAddress = "0xffb4fc4b54415fec990186733c656935496c8b79"
                },

                new Network
                {
                    Code = NetworkCode.TRX, Name = "Tron (TRC20)",
                    WalletAddress = "TDx9nzwoZXH5LMesVN98EGdY9xnf6W6tge"
                }
            ]
        },

        new DigitalAsset
        {
            Value = Currency.TRX,
            Icon = "🔺",
            Label = "Tron",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.TRX, Name = "Tron (TRC20)",
                    WalletAddress = "TDx9nzwoZXH5LMesVN98EGdY9xnf6W6tge"
                }
            ]
        },

        new DigitalAsset
        {
            Value = Currency.BTC,
            Icon = "🪙",
            Label = "Bitcoin",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.BTC, Name = "Bitcoin", WalletAddress =
                        "1NzWPKKJNjpHaJCep9jsGZ8uQh86d5HvwM"
                }
            ]
        },

        new DigitalAsset
        {
            Value = Currency.LTC,
            Icon = "⚡",
            Label = "Litecoin",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.LTC, Name = "Litecoin",
                    WalletAddress = "ltc1qrgpd7jm7dw3uhhprdnsszhelg4hcz5hy2ar9zd"
                }
            ]
        },

        new DigitalAsset
        {
            Value = Currency.ETH,
            Icon = "💎",
            Label = "Ethereum",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.ETH, Name = "Ethereum (ERC20)",
                    WalletAddress = "0xffb4fc4b54415fec990186733c656935496c8b79"
                }
            ]
        },
        new DigitalAsset
        {
            Value = Currency.USD,
            Icon = "💳",
            Label = "USD",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.CARD, Name = "USD Карта",
                    WalletAddress = ""
                },
                new Network
                {
                    Code = NetworkCode.CASH, Name = "USD Наличные",
                    WalletAddress = ""
                }
            ]
        },
        new DigitalAsset
        {
            Value = Currency.EUR,
            Icon = "💳",
            Label = "EUR",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.CARD, Name = "EUR Карта",
                    WalletAddress = ""
                },
                new Network
                {
                    Code = NetworkCode.CASH, Name = "EUR Наличные",
                    WalletAddress = ""
                }
            ]
        },
        new DigitalAsset
        {
            Value = Currency.MDL,
            Icon = "💳",
            Label = "MDL",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.CARD, Name = "MDL Карта",
                    WalletAddress = ""
                },
                new Network
                {
                    Code = NetworkCode.CASH, Name = "MDL Наличные",
                    WalletAddress = ""
                }
            ]
        },
        new DigitalAsset
        {
            Value = Currency.RUP,
            Icon = "💵",
            Label = "Рубль ПМР",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.CARD, Name = "RUP Карта",
                    WalletAddress = ""
                },
                new Network
                {
                    Code = NetworkCode.CASH, Name = "RUP Наличные",
                    WalletAddress = ""
                }
            ]
        },
        new DigitalAsset
        {
            Value = Currency.RUB,
            Icon = "💵",
            Label = "Рубль РФ",
            Networks =
            [
                new Network
                {
                    Code = NetworkCode.CARD, Name = "RUB Карта",
                    WalletAddress = ""
                },
                new Network
                {
                    Code = NetworkCode.CASH, Name = "RUB Наличные",
                    WalletAddress = ""
                }
            ]
        }
    ];

    public static Network GetNetworkByCurrency(Currency currency, NetworkCode networkCode)
    {
        return Currencies
            .Where(x => x.Value == currency)
            .SelectMany(x => x.Networks)
            .FirstOrDefault(x => x.Code == networkCode)!;
    }
}