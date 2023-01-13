using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using AivaptDotNet.Helpers.General;
using AivaptDotNet.Handlers;
using AivaptDotNet.Services;
using AivaptDotNet.Services.Database;
using Victoria;
using Discord.Interactions;

namespace AivaptDotNet
{
    public class Bot
    {
        #region Fields

        private IServiceProvider _services;
        private Credentials _credentials;

        #endregion

        #region Constructor

        public Bot()
        {
            GetConfiguration();
            ConfigureServices();
        }

        #endregion

        #region Methods

        #region Public Methods

        public async Task StartBot()
        {
            var botClient = _services.GetRequiredService<DiscordSocketClient>();
            var commandHandler = _services.GetRequiredService<CommandHandler>();
            var interactionHandler = _services.GetRequiredService<InteractionHandler>();

            ConfigureBot();

            await botClient.LoginAsync(TokenType.Bot, _credentials.BotToken);
            await botClient.StartAsync();

            await commandHandler.InitializeCommands();
            await interactionHandler.InitializeCommands();

            await Task.Delay(-1);
        }

        #endregion

        #region Private Methods

        private void ConfigureServices()
        {
            _services = new ServiceCollection()
                .AddDbContext<BotDbContext>( options =>
                        options.UseMySql(_credentials.DbConnectionString, ServerVersion.AutoDetect(_credentials.DbConnectionString)))
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>()
                .AddSingleton<IAudioService, VictoriaAudioService>()
                .AddLavaNode(lc => { lc.SelfDeaf = true; lc.Authorization = _credentials.LavalinkPassword; lc.Hostname = "lavalink"; })
                .AddMemoryCache()
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
                LavalinkPassword = aivaptSection["LAVALINK_PASSWORD"],
                DebugGuildId = Convert.ToUInt64(aivaptSection["DEBUG_GUILD_ID"])
            };
        }

        private void ConfigureBot()
        {
            var botClient = _services.GetRequiredService<DiscordSocketClient>();
            var commands = _services.GetRequiredService<CommandService>();

            botClient.Ready += OnBotReady;
            botClient.Log += Logging;
            commands.Log += Logging;

            DiscordSocketConfig clientConfig = new DiscordSocketConfig
            {
                MessageCacheSize = 50,
                AlwaysDownloadUsers = true
            };
        }

        #endregion

        #endregion

        #region Events

        private Task Logging(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task OnBotReady()
        {
            var lavaNode = _services.GetRequiredService<LavaNode>();
            var interactions = _services.GetRequiredService<InteractionService>();

            await interactions.RegisterCommandsGloballyAsync();

            if (!lavaNode.IsConnected)
                await lavaNode.ConnectAsync();
        }

        #endregion
    }
}
