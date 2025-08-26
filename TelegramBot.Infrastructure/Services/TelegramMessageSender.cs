using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces;

namespace TelegramBot.Infrastructure.Services;

public class TelegramMessageSender : IMessageSender
{
    private readonly ITelegramBotClient _botClient;

    public TelegramMessageSender(ITelegramBotClient telegramBotClient)
    {
        _botClient = telegramBotClient;
    }
    public Task SendTextAsync(long chatId, string message, CancellationToken cancellationToken, ParseMode parse = ParseMode.None)
    {
        return _botClient.SendMessage(
            chatId, 
            message, 
            parseMode: parse,
            cancellationToken: cancellationToken);
    }
}