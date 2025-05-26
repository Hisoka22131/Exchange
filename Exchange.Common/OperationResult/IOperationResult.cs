using Exchange.Common.OperationResult.Error;

namespace Exchange.Common.OperationResult;

public interface IOperationResult
{
    public interface IOperationResult
    {
        IReadOnlyCollection<OperationError>? Errors { get; }
        bool Ok { get; }
    }

    public interface IOperationResult<out TResult> : IOperationResult
    {
        TResult? Result { get; }
    }
}