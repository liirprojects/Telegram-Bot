using TelegramBot.Application.Interfaces;

// using needed for class to work
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;

namespace TelegramBot.Infrastructure.Services;

public class MessageService : IMessageService
{
    private readonly IEnumerable<ICommandHandler> _handlers;
    private readonly IMessageSender _messageSender;
    private readonly ILogger<MessageService> _logger;

    public MessageService(IEnumerable<ICommandHandler> handlers, IMessageSender sender, ILogger<MessageService> logger)
    {
        _handlers = handlers;
        _messageSender = sender;
        this._logger = logger;
    }
    public async Task HandleAsync(string message, long chatId, CancellationToken cancellationTocken)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            _logger.LogWarning("Received empty message from chatId: {ChatId}", chatId);
            return;
        }

        string command = message.Trim().Split(' ', '\n')[0].ToLowerInvariant();

        var handler = _handlers.FirstOrDefault(h => h.Command == command);

        if (handler is null)
        {
            await _messageSender.SendTextAsync(chatId,
                "❓ Unknown command. Type /help to see available commands.",
                cancellationTocken);
            return;
        }
        await handler.HandleAsync(chatId, message, cancellationTocken);
    }
}