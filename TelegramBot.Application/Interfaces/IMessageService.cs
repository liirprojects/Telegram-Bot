namespace TelegramBot.Application.Interfaces;

public interface IMessageService
{
    Task HandleAsync(string message, long chatId, CancellationToken cancellationTocken);
}