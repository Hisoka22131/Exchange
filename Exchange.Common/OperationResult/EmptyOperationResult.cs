using Exchange.Common.OperationResult.Error;

namespace Exchange.Common.OperationResult;

public class EmptyOperationResult : OperationResult
{
    public static implicit operator EmptyOperationResult(OperationError? error)
    {
        if (error is null) return new EmptyOperationResult();
        
        var result = new EmptyOperationResult();
        result.AddError(error);
        return result;
    }
    
    public static implicit operator EmptyOperationResult(OperationError[]? errors)
    {
        if (errors is null) return new EmptyOperationResult();
        
        var result = new EmptyOperationResult();
        result.AddErrors(errors);
        return result;
    }
    
    public static implicit operator EmptyOperationResult(List<OperationError>? errors)
    {
        if (errors is null) return new EmptyOperationResult();
        
        var result = new EmptyOperationResult();
        result.AddErrors(errors);
        return result;
    }
}