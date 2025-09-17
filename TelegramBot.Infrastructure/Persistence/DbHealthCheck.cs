using System.Data.Common;
using TelegramBot.Application.Interfaces;
namespace TelegramBot.Infrastructure.Persistence;

// Tests and checks the connection with SQL SERVER
public sealed class DbHealthCheck
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DbHealthCheck(ISqlConnectionFactory sqlConnectionFactory)
    {
        _connectionFactory = sqlConnectionFactory;
    }

    public async Task<bool> CanConnectAsync(CancellationToken ct = default)
    {
        try
        {
            await using DbConnection conn = _connectionFactory.Create();
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1";

            var x = await cmd.ExecuteScalarAsync(ct);

            return x is int i && i == 1;
        }
        catch { return false; }
    }
}