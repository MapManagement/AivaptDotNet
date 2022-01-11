using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using AivaptDotNet.Helpers;
using AivaptDotNet.Handlers;
using AivaptDotNet.Services;

namespace AivaptDotNet 
{
    public class Bot
    {
        #region Fields
        
        private  IServiceProvider _services; 
        private readonly DiscordSocketClient _botClient;
        private readonly CommandService _commands;
        private readonly CommandHandler _commandHandler;
        private readonly DatabaseService _dbService;
        private readonly CacheService _cacheService;

        #endregion

        #region Constructor

        public Bot()
        {
            ConfigureServices();
            _botClient = _services.GetRequiredService<DiscordSocketClient>();
            _commandHandler = _services.GetRequiredService<CommandHandler>();
            _dbService = _services.GetRequiredService<DatabaseService>();
            _cacheService = _services.GetRequiredService<CacheService>();

            _botClient.Ready += OnBotReady;
            _botClient.Log += Logging;

            DiscordSocketConfig clientConfig = new DiscordSocketConfig
            {
                MessageCacheSize = 50,
                AlwaysDownloadUsers = true
            };
        }

        #endregion

        #region Public Methods

        public async Task StartBot()
        {
            await _commandHandler.InitializeCommands();
            await _dbService.Initialize(Credentials.GetDbConnection());
            _cacheService.Initialize(120);

            await _botClient.LoginAsync(TokenType.Bot, Credentials.GetBotToken());
            await _botClient.StartAsync();

            await Task.Delay(-1);
        }

        #endregion

        #region Private Methods

        private void ConfigureServices()
        {
            _services = new ServiceCollection()
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandler>()
            .AddSingleton<DatabaseService>()
            .AddSingleton<CacheService>()
            .BuildServiceProvider();
        }

        #endregion

        #region Events

        private Task Logging(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task OnBotReady()
        {
            await _botClient.SetActivityAsync(new GameActivity("Sudoku", "Almost finished..."));
        }

        #endregion
    }
}