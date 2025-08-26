using Telegram.Bot.Types.Enums;
namespace TelegramBot.Application.Interfaces;

public interface IMessageSender
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chatId">The chat ID to send the message to.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendTextAsync(long chatId, string message, CancellationToken cancellationToken, ParseMode parse = ParseMode.None);
}
