namespace Exchange.Web.Blazor.Services.Users.Dto;

public class UserBlazorQueryDto
{
    public int? Count { get; set; }
    public int? Offset { get; set; }
    public Guid? UserId { get; set; }
    public string? TelegramUserName { get; set; }
}