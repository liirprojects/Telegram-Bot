using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TelegramBot.Application.Interfaces;

namespace TelegramBot.Infrastructure.Persistence;

// Get connection string from appsettings.json and user secter to create SqlConnection
public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _cs;

    public SqlConnectionFactory(IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionStrings:Default is missing");
    }

    public DbConnection Create() => new SqlConnection(_cs);
}