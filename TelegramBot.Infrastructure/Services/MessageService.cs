using TelegramBot.Application.Interfaces;

// using needed for class to work
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;

namespace TelegramBot.Infrastructure.Services;

public class MessageService : IMessageService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<MessageService> _logger;

    public MessageService(ITelegramBotClient telegramBotClient, ILogger<MessageService> logger)
    {
        this._telegramBotClient = telegramBotClient;
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

        switch(command)
            {
            case "/start":
                await OnStartAsync(chatId, cancellationTocken);
                break;
            case "/help":
                await OnHelpAsync(chatId, cancellationTocken);
                break;
            default:
                _logger.LogWarning("Unknown command '{Command}' from chatId: {ChatId}", command, chatId);
                await _telegramBotClient.SendMessage(
                    chatId: chatId,
                    text: "Unknown command. Type /help to see available commands.",
                    parseMode: ParseMode.None,
                    cancellationToken: cancellationTocken);
                break;
        }
    }
    #region
    private Task OnStartAsync(long chatId, CancellationToken cancellationToken) =>
        _telegramBotClient.SendMessage(
            chatId: chatId,
            text: "Hello! I'm your bot 🤖. Type /help to find out what I can do!",
            parseMode: ParseMode.None,
            cancellationToken: cancellationToken);

    private Task OnHelpAsync(long chatId, CancellationToken cancellationToken) =>
        _telegramBotClient.SendMessage(
            chatId: chatId,
            text: "**I can help you with the following commands:**\n" +
                "/start - Start the bot\n" +
                "/help - Show this help message\n",
            parseMode: ParseMode.Markdown,
            cancellationToken: cancellationToken);
    #endregion
}