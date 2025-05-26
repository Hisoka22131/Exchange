namespace Exchange.Database.Models;

public class DataProtectionKey
{
    public int Id { get; set; }
    public string FriendlyName { get; set; } = null!;
    public string Xml { get; set; } = null!;
}