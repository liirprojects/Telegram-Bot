
using System.Threading;
using TelegramBot.Application.Interfaces;
using TelegramBot.Infrastructure.Services;
using TelegramBot.Application.Localization;

namespace TelegramBot.Infrastructure.Commands;

public class StartCommandHandler : CommandHandler
{
    public override string Command => "start";

    public StartCommandHandler(IMessageSender _messageSender) : base(_messageSender) { }

    public override Task HandleAsync(long chatId, string message, CancellationToken ct)
    {
        return _messageSender.SendTextAsync(chatId, Texts.Start, ct);
    }
}