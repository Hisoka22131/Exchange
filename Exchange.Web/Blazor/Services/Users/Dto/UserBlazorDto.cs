namespace Exchange.Web.Blazor.Services.Users.Dto;

public class UserBlazorDto
{
    public Guid Id { get; set; }
    public long TelegramUserId { get; init; }
    public string TelegramUserName { get; init; }
}