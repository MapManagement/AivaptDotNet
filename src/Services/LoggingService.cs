using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;

namespace AivaptDotNet.Services
{
    public class LoggingService
    {
        #region Fields

        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _botClient;
        private readonly InteractionService _interactionService;
        private readonly CommandService _commandService;

		#endregion

		#region Constructor

        public LoggingService(IServiceProvider services)
        {
            _services = services;
            _botClient = services.GetRequiredService<DiscordSocketClient>();
            _interactionService = services.GetRequiredService<InteractionService>();
            _commandService = services.GetRequiredService<CommandService>();
        }

		#endregion

        #region Methods

        #region Public

        public void InitializeLogging()
        {
            AddLoggingEvents();
        }

        #endregion

        #region Private

        private void AddLoggingEvents()
        {
            _interactionService.Log += LogAsync;
            _botClient.Log += LogAsync;
            _commandService.Log += LogAsync;
        }

        #endregion

        #endregion

        #region Events

        private Task LogAsync(LogMessage message)
        {
            if (message.Severity == LogSeverity.Error)
            {   
                Console.WriteLine(message.Severity.ToString());
                Console.WriteLine(message.Exception);
            }

            Console.WriteLine(message.Message); 

            return Task.CompletedTask;
        }

        #endregion

    }
}
