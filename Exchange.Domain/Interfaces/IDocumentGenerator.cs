namespace Exchange.Domain.Interfaces;

public interface IDocumentGenerator
{
    byte[] CreateExcelFile<T>(IEnumerable<T> entities);
}