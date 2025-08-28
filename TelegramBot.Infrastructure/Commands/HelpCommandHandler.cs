using TelegramBot.Application.Interfaces;
using TelegramBot.Infrastructure.Services;
using TelegramBot.Application.Localization;

namespace TelegramBot.Infrastructure.Commands;

public class HelpCommandHandler : CommandHandler
{ 
    public override string Command => "help";

    public HelpCommandHandler(IMessageSender _messageSender) : base(_messageSender) { }

    public override Task HandleAsync(long chatId, string message, CancellationToken ct)
    {
        return _messageSender.SendTextAsync(chatId, Texts.Help, ct);
    }
}
