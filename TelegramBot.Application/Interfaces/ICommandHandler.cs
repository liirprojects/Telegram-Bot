namespace TelegramBot.Application.Interfaces;

// Contract of a single bot command handler 
public interface ICommandHandler
{
    /// <summary> Command text, for example "/start".</summary>
    string Command { get; }

    /// <summary> Command description, for example "Start bot".</summary>
    string Description { get; }

    /// <summary> Command Handler. Message — whole users text.</summary>
    Task HandleAsync(long chatId, string message, CancellationToken ct);
}
