
using System.Threading;
using TelegramBot.Application.Interfaces;
using TelegramBot.Infrastructure.Services;

namespace TelegramBot.Infrastructure.Commands;

public class StartCommandHandler : CommandHandler
{
    public override string Command => "/start";

    public StartCommandHandler(IMessageSender _messageSender) : base(_messageSender) { }

    public override Task HandleAsync(long chatId, string message, CancellationToken ct)
    {
        return _messageSender.SendTextAsync(chatId, "👋 Hello! I'm your bot.", ct);
    }
}