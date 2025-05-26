using Exchange.Common.Enums;

namespace Exchange.Domain.Interfaces;

public interface IExchangeMetaData
{
    Currency GetCurrencyFrom();
    Currency GetCurrencyTo();
}