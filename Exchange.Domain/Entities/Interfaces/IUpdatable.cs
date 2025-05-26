namespace Exchange.Domain.Entities.Interfaces;

public interface IUpdatable
{
    DateTimeOffset UpdatedAt { get; set; }
}