using TelegramBot.Application.Interfaces;
using TelegramBot.Infrastructure.Services;
using TelegramBot.Infrastructure.Persistence;

namespace TelegramBot.Infrastructure.Commands;

public class AddCommandHandler : CommandHandler
{
    public override string Command => "add";
    public override string Description => "Add a new task";

    private readonly TaskRepository _repo;

    public AddCommandHandler(IMessageSender messageSender, TaskRepository repo) : base(messageSender)
    {
        _repo = repo;
    }
    public override async Task HandleAsync(long chatId, string message, CancellationToken ct)
    {
        var parts = (message ?? string.Empty).Split(' ',2, StringSplitOptions.RemoveEmptyEntries);

        if(parts.Length < 2)
        {
           await _messageSender.SendTextAsync(chatId, "Usage: /add <task description>", ct);
           return;
        }

        var taskText = parts[1];

        try 
        {
            await _repo.AddAsync(chatId, taskText, ct);
            await _messageSender.SendTextAsync(chatId, $"Task added: {taskText}", ct);
        }
        catch (Exception ex)
        {
            // можно логировать ex.ToString() в ILogger
            await _messageSender.SendTextAsync(chatId, "Failed to add task. Please try again.", ct);
        }

    }
}