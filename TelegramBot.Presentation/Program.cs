 using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Requests;
using TelegramBot.Application.Interfaces;
using TelegramBot.Infrastructure.Services;
using TelegramBot.Presentation.Telegram;
using TelegramBot.Infrastructure.Commands;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

var builder = Host.CreateApplicationBuilder(args);

// (user-secrets/ENV/appsettings)
var token = builder.Configuration["Telegram:BotToken"];
if (string.IsNullOrWhiteSpace(token))
    throw new InvalidOperationException("Telegram:BotToken is not configured. " +
                                            "Use user-secrets or env TELEGRAM__BOTTOKEN");

// 2) Setting up simple logs in the console
builder.Services.AddLogging(x =>
{
    x.ClearProviders();
    x.AddSimpleConsole(o =>
    {
        o.SingleLine = true;
        o.TimestampFormat = "HH:mm:ss ";
    });
    x.SetMinimumLevel(LogLevel.Information);
});

// 3) Registering a Telegram client the “right” way via HttpClientFactory
builder.Services.AddHttpClient("tg")
    .AddTypedClient<ITelegramBotClient>((http, sp) => new TelegramBotClient(token, http));

// 4) Add services
builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IMessageSender, TelegramMessageSender>();
builder.Services.AddSingleton<ICommandHandler, StartCommandHandler>();
builder.Services.AddSingleton<ICommandHandler, HelpCommandHandler>();

// 5) A background service that starts receiving messages and stops gracefully.
builder.Services.AddHostedService<TelegramPollingHostedService>();

var app = builder.Build();
await app.RunAsync();

/// Background service - runs long-polling Telegram and keeps the bot "alive"
public sealed class TelegramPollingHostedService : BackgroundService
{
    private readonly ITelegramBotClient _bot;
    private readonly IUpdateHandler _handler;
    private readonly ILogger<TelegramPollingHostedService> _log;

    private readonly IEnumerable<ICommandHandler> _handlers;

    public TelegramPollingHostedService( ITelegramBotClient bot, IUpdateHandler handler,
        ILogger<TelegramPollingHostedService> log, IEnumerable<ICommandHandler> handlers)
    {
        _bot = bot;
        _handler = handler;
        _log = log;
        _handlers = handlers;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // accept all update types
        };

        _log.LogInformation("Starting Telegram long-polling…");
        _bot.StartReceiving(_handler, receiverOptions, stoppingToken);

        var me = await _bot.GetMe(stoppingToken);
        _log.LogInformation("Bot ready: @{Username} (id {Id})", me.Username, me.Id);

        var cmds = _handlers.Select(h => new Telegram.Bot.Types.BotCommand
        {
            Command = h.Command,
            Description = h.Description
        }).ToArray();

        await _bot.SetMyCommands(cmds, cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
