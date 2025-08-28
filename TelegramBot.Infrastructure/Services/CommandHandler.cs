
using TelegramBot.Application.Interfaces;

namespace TelegramBot.Infrastructure.Services;

public abstract class CommandHandler : ICommandHandler
{
    public abstract string Command { get; }
    public virtual string Description => string.Empty;

    protected readonly IMessageSender _messageSender;

    protected CommandHandler(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }
    public abstract Task HandleAsync(long chatId, string message, CancellationToken ct);
}