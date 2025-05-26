namespace Exchange.Domain.Entities.Interfaces;

public interface ICreatable
{
    public DateTimeOffset CreatedAt { get; set; }
}