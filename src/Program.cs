using AivaptDotNet.Handlers;
using AivaptDotNet.Helpers.General;
using AivaptDotNet.Services;
using AivaptDotNet.Services.Database;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Victoria;

namespace AivaptDotNet
{
    public class Program
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;
        private Credentials _credentials;

        #endregion

        #region Constructor

        public Program()
        {
            _credentials = GetConfiguration();
            _serviceProvider = CreateProvider();
        }

        #endregion

        #region Methods

        #region Public

        public static void Main(string[] args)
        {
            new Program().RunAsync().GetAwaiter().GetResult();
        }

        #endregion

        #region Private

        private async Task RunAsync()
        {
            var bot = _serviceProvider.GetService<DiscordSocketClient>();

            var botClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
            var commandHandler = _serviceProvider.GetRequiredService<CommandHandler>();
            var interactionHandler = _serviceProvider.GetRequiredService<InteractionHandler>();

            ConfigureBot();

            await botClient.LoginAsync(TokenType.Bot, _credentials.BotToken);
            await botClient.StartAsync();

            await commandHandler.InitializeCommands();
            await interactionHandler.InitializeCommands();

            await Task.Delay(-1);
        }
        
        // TODO: move to dedicated class
        private void ConfigureBot()
        {
            var botClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
            var commands = _serviceProvider.GetRequiredService<CommandService>();

            botClient.Ready += OnBotReady;
            botClient.Log += Logging;
            commands.Log += Logging;

            DiscordSocketConfig clientConfig = new DiscordSocketConfig
            {
                MessageCacheSize = 50,
                AlwaysDownloadUsers = true
            };
        }


        private IServiceProvider CreateProvider()
        {
            var services = new ServiceCollection()
                .AddDbContext<BotDbContext>(options =>
                        options.UseMySql(_credentials.DbConnectionString, new MariaDbServerVersion(new Version(10, 9, 4))))
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>()
                .AddSingleton<IAudioService, VictoriaAudioService>()
                .AddLavaNode(lc =>
                        {
                            lc.SelfDeaf = true;
                            lc.Authorization = _credentials.LavalinkPassword;
                            lc.Hostname = "lavalink";
                        })
                .AddMemoryCache()
                .BuildServiceProvider();

            return services;
        }

        private Credentials GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            var aivaptSection = config.GetSection("AIVAPT");

            var credentials = new Credentials
            {
                AdminId = Convert.ToUInt64(aivaptSection["ADMIN_ID"]),
                BotToken = aivaptSection["BOT_TOKEN"],
                DbConnectionString = aivaptSection["DB_CONNECTION_STRING"],
                LavalinkPassword = aivaptSection["LAVALINK_PASSWORD"],
                DebugGuildId = Convert.ToUInt64(aivaptSection["DEBUG_GUILD_ID"])
            };

            return credentials;
        }

        #endregion

        #endregion

        #region Events

        // TODO: move to dedicated class
        private Task Logging(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        // TODO: move to dedicated class
        private async Task OnBotReady()
        {
            var lavaNode = _serviceProvider.GetRequiredService<LavaNode>();
            var interactions = _serviceProvider.GetRequiredService<InteractionService>();

            await interactions.RegisterCommandsGloballyAsync();

            if (!lavaNode.IsConnected)
                await lavaNode.ConnectAsync();
        }

        #endregion
    }
}
