using Exchange.Domain.Entities;

namespace Exchange.Web.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public long TelegramUserId { get; init; }
    public string TelegramUserName { get; init; }
    
    public UserDto(UserEntity entity)
    {
        Id = entity.Id;
        TelegramUserId = entity.TelegramUserId;
        TelegramUserName = entity.TelegramUserName;
    }
}