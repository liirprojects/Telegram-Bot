using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Application.Interfaces;

namespace TelegramBot.Infrastructure.Services;

public class TelegramMessageSender : IMessageSender
{
    private readonly ITelegramBotClient _botClient;

    public TelegramMessageSender(ITelegramBotClient telegramBotClient)
    {
        _botClient = telegramBotClient;
    }
    public Task SendTextAsync(long chatId, string message, CancellationToken cancellationToken, 
        ParseMode parse = ParseMode.None, ReplyMarkup? replyMarkup = null)
    {
        return _botClient.SendMessage(
            chatId, 
            message, 
            parseMode: parse,
            replyMarkup: replyMarkup,
            cancellationToken: cancellationToken);
    }
}