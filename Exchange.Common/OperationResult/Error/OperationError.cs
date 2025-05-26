namespace Exchange.Common.OperationResult.Error;

public class OperationError
{
    public string ErrorCode { get; }

    public string ErrorMessage { get; }

    public OperationError(string errorCode, string errorMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(errorCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}