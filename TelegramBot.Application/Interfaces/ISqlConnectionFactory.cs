using System.Data.Common;

namespace TelegramBot.Application.Interfaces;

public interface ISqlConnectionFactory
{
    DbConnection Create();
}