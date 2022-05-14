using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using AivaptDotNet.Helpers.General;
using AivaptDotNet.Handlers;
using AivaptDotNet.Services;

using Victoria;

namespace AivaptDotNet 
{
    public class Bot
    {
        #region Fields
        
        private IServiceProvider _services; 

        private readonly DiscordSocketClient _botClient;

        private readonly CommandService _commands;

        private readonly CommandHandler _commandHandler;

        private readonly DatabaseService _dbService;

        private readonly CacheService _cacheService;

        private readonly IAudioService _audioService;

        private readonly EventService _eventService;

        private readonly LavaNode _lavaNode;

        private Credentials _credentials;

        #endregion

        #region Constructor

        public Bot()
        {
            GetConfiguration();

            ConfigureServices();
            _botClient = _services.GetRequiredService<DiscordSocketClient>();
            _commandHandler = _services.GetRequiredService<CommandHandler>();
            _dbService = _services.GetRequiredService<DatabaseService>();
            _cacheService = _services.GetRequiredService<CacheService>();
            _audioService = _services.GetRequiredService<IAudioService>();
            _eventService = _services.GetRequiredService<EventService>();
            _lavaNode = _services.GetRequiredService<LavaNode>();

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
            await _botClient.LoginAsync(TokenType.Bot, _credentials.BotToken);
            await _botClient.StartAsync();

            await _commandHandler.InitializeCommands();
            await _dbService.Initialize(_credentials.DbConnectionString);
            _cacheService.Initialize(120);

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
            .AddSingleton<IAudioService, VictoriaAudioService>()
            .AddSingleton<EventService>()
            .AddLavaNode(lc => { lc.SelfDeaf = true; lc.Authorization = _credentials.LavalinkPassword; lc.Hostname = "lavalink"; })
            .BuildServiceProvider();
        }

        private void GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>(optional: true)
                .AddEnvironmentVariables()
                .Build();
            var aivaptSection = config.GetSection("AIVAPT");
            
            _credentials = new Credentials
            {
                AdminId = Convert.ToUInt64(aivaptSection["ADMIN_ID"]),
                BotToken = aivaptSection["BOT_TOKEN"],
                DbConnectionString = aivaptSection["DB_CONNECTION_STRING"],
                LavalinkPassword = aivaptSection["LAVALINK_PASSWORD"]
            };
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
            //await _botClient.SetActivityAsync(new DefaultActivity("Sudoku", "Almost finished..."));

            if(!_lavaNode.IsConnected) await _lavaNode.ConnectAsync(); 
        }

        #endregion
    }
}