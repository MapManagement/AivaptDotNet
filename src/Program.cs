using AivaptDotNet.DataClasses;
using AivaptDotNet.Handlers;
using AivaptDotNet.Services;
using AivaptDotNet.Services.Database;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Victoria;

namespace AivaptDotNet
{
    public class Program
    {
        #region Fields

        private static IServiceProvider _serviceProvider;
        private static Credentials _credentials;

        #endregion

        #region Methods

        #region Public

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            _serviceProvider = host.Services;
            new Program().RunAsync().GetAwaiter().GetResult();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            _credentials = GetConfiguration();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(CreateProvider);

            return host;
        }

        #endregion

        #region Private

        private async Task RunAsync()
        {
            var botClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
            var commandHandler = _serviceProvider.GetRequiredService<CommandHandler>();
            var interactionHandler = _serviceProvider.GetRequiredService<InteractionHandler>();
            var logging = _serviceProvider.GetRequiredService<LoggingService>();

            logging.InitializeLogging();

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

            DiscordSocketConfig clientConfig = new DiscordSocketConfig
            {
                MessageCacheSize = 50,
                AlwaysDownloadUsers = true
            };
        }

        private static void CreateProvider(HostBuilderContext context, IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddDbContext<BotDbContext>(options =>
                    options.UseMySql(_credentials.DbConnectionString,
                                     new MariaDbServerVersion(new Version(10, 9, 4))))
                .AddSingleton<LoggingService>()
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
        }

        private static Credentials GetConfiguration()
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
