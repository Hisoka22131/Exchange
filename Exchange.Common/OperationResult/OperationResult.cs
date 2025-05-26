using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Exchange.Common.OperationResult.Error;

namespace Exchange.Common.OperationResult;

public abstract class OperationResult : IOperationResult
{
    private List<OperationError>? _errors;
    
    public IReadOnlyCollection<OperationError>? Errors => _errors?.AsReadOnly();

    [MemberNotNullWhen(false, $"{nameof(_errors)}", $"{nameof(Errors)}")]
    public virtual bool Ok
    {
        [MemberNotNullWhen(false, $"{nameof(_errors)}", $"{nameof(Errors)}")] get => _errors is null || _errors.Count is 0;
    }

    public virtual void AddError(OperationError operationError)
    {
        if (Ok) _errors = new List<OperationError>();
        _errors.Add(operationError);
    }
    
    public virtual void AddErrors(IEnumerable<OperationError> errors)
    {
        if (Ok) _errors = new List<OperationError>();
        _errors.AddRange(errors);
    }
}

public class OperationResult<TResult> : OperationResult, IOperationResult.IOperationResult<TResult>
{
    public TResult? Result { get; set; }

    [MemberNotNullWhen(true, $"{nameof(Result)}")]
    public override bool Ok
    {
        [MemberNotNullWhen(true, $"{nameof(Result)}")] get => base.Ok;
    }
    
    public static implicit operator OperationResult<TResult>(OperationError? error)
    {
        if (error is null) return new OperationResult<TResult>();
        
        var result = new OperationResult<TResult>();
        result.AddError(error);
        return result;
    }
    
    public static implicit operator OperationResult<TResult>(OperationError[]? errors)
    {
        if (errors is null) return new OperationResult<TResult>();
        
        var result = new OperationResult<TResult>();
        result.AddErrors(errors);
        return result;
    }
    
    public static implicit operator OperationResult<TResult>(List<OperationError>? errors)
    {
        if (errors is null) return new OperationResult<TResult>();
        
        var result = new OperationResult<TResult>();
        result.AddErrors(errors);
        return result;
    }
    
    public static implicit operator OperationResult<TResult>(EmptyOperationResult emptyOperationResult)
    {
        if (emptyOperationResult.Errors is null) return new OperationResult<TResult>();
        
        var result = new OperationResult<TResult>();
        result.AddErrors(emptyOperationResult.Errors);
        return result;
    }
}