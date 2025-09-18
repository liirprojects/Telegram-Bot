using System.Data.Common;
using TelegramBot.Application.Interfaces;

namespace TelegramBot.Infrastructure.Persistence;

public sealed class TaskRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    public TaskRepository(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task AddAsync(long userId, string text, CancellationToken ct = default)
    {
        await using DbConnection conn = _sqlConnectionFactory.Create();
        await conn.OpenAsync(ct);

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Tasks dbo.Tasks (UserId, [Text]) VALUES (@u, @t)";

        // Creating parameter for UserId
        var p1 = cmd.CreateParameter(); p1.ParameterName = "@u";
        p1.Value = userId;
        cmd.Parameters.Add(p1);

        //Crrating parameter for Text
        var p2 = cmd.CreateParameter(); p2.ParameterName = "@t";
        p2.Value = text;
        cmd.Parameters.Add(p2);

        await cmd.ExecuteNonQueryAsync(ct);
    }
}