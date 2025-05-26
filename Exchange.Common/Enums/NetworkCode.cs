namespace Exchange.Common.Enums;

public enum NetworkCode : byte
{
    UNKNOWN = 0,
    
    BSC = 1,
    ETH = 2,
    TRX = 3,
    BTC = 4,
    LTC = 5,
    
    CASH = 6,
    CARD = 7
}

public static class NetworkExtensions
{
    public static bool IsCash(this NetworkCode code)
    {
        return code switch
        {
            NetworkCode.CASH => true,
            _ => false
        };
    }
}