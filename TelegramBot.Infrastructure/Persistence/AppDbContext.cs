using Microsoft.EntityFrameworkCore;
using TelegramBot.Application.Domain;

namespace TelegramBot.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();
}