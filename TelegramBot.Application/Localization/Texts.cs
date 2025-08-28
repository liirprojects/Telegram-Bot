using TelegramBot.Application.Localization;

namespace TelegramBot.Application.Localization; 
public static class Texts 
{ 
    public const string Unknown = "❓ Unknown command." +
        " Type /help to see available commands.";

    public const string Start = "Hi! I'm your bot. Type /help to see what I can do.";
    public const string Help = "Available commands:\n/start — greet\n/help — show this help";

}
