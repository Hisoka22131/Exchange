using Exchange.Common.OperationResult.Error;

namespace Exchange.Web.Resources;

public static class ApiErrors
{
    private const string CommonErrorCode = "exchange.api";
    
    public static OperationError InvalidCredentials => new(CommonErrorCode, "Invalid username or password");
    public static OperationError Unauthorized => new(CommonErrorCode, "Необходимо авторизоваться. Срок действия токена истек или токен недействителен");
    public static OperationError Exception(string message) => new(CommonErrorCode, message);
}