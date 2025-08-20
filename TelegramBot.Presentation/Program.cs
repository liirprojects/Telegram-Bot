using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Interfaces;
using TelegramBot.Infrastructure.Services;
using TelegramBot.Presentation.Telegram;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var botToken = context.Configuration["Telegram:BotToken"]
                    ?? throw new InvalidOperationException("Telegram:BotToken not found in configuration");

    services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

    services.AddSingleton<IMessageService, MessageService>();
    services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
});

var app = builder.Build();

var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
var updateHandler = app.Services.GetRequiredService<IUpdateHandler>();
var cancellationToken = new CancellationTokenSource().Token;

botClient.StartReceiving(
        updateHandler: updateHandler,
        receiverOptions: new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // Receive all update types
        },
        cancellationToken: cancellationToken
    );

Console.WriteLine("✅ The bot is running. Press Ctrl+C to stop.");
await Task.Delay(-1);