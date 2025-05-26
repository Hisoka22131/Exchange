using Exchange.Common.OperationResult.Error;

namespace Exchange.Common.Resources;

public static class CommonErrors
{
    private const string CommonErrorCode = "exchange.common";
    
    public static OperationError CommonError(string message) => new(CommonErrorCode, message);
}