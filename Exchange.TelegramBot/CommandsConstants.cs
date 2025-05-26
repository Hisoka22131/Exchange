namespace Exchange.TelegramBot;

public static class CommandsConstants
{
    public static class Common
    {
        public const string Start = "/start";
        public const string Help = "/help";
        public const string GetListOfContacts = "/get_list_of_contacts";
        public const string GetChatId = "/get_chat_id";
    }

    public static class User
    {
        public const string GetMyProfile = "/get_my_profile";
    }

    public static class Transactions
    {
        public const string Confirm = "/confirm";
        public const string Reject = "/reject";
    }
}