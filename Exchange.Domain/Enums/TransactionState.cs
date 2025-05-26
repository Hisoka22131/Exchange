using System.ComponentModel;

namespace Exchange.Domain.Enums;

public enum TransactionState : byte
{
    Unknown = 0,
    
    [Description("В процессе")]
    Processing = 1,
    
    [Description("Завершена")]
    Confirmed = 2,
    
    [Description("Отклонена")]
    Rejected = 3,
    
    [Description("Начата")]
    Init = 4
}