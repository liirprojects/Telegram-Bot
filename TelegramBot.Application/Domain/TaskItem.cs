using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Application.Domain;

public sealed class TaskItem
{
    public int Id { get; set; }
    public long UserId { get; set; }
    
    [MaxLength(256)]
    public string Text { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}