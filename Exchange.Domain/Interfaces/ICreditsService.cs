namespace Exchange.Domain.Interfaces;

public interface ICreditsService
{
    Task<int> CalculateCreditsAsync(CancellationToken ct);
}