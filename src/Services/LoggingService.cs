using System;
using System.Text;
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

        private void LogError(LogMessage message)
        {
            var errorText = new StringBuilder("==== ERROR ====")
                .Append(Environment.NewLine)
                .Append($"Time: {GetCurrentDateTime()}")
                .Append(Environment.NewLine)
                .Append($"Source: {message.Source}")
                .Append(Environment.NewLine)
                .Append($"Exception: {message.Exception}")
                .ToString();

            Console.WriteLine(errorText);
        }

        private void LogDebugging(LogMessage message)
        {
            var debugText = new StringBuilder("==== DEBUG ====")
                .Append(Environment.NewLine)
                .Append($"Time: {GetCurrentDateTime()}")
                .Append(Environment.NewLine)
                .Append($"Source: {message.Source}")
                .Append(Environment.NewLine)
                .Append($"Message: {message.Message}")
                .ToString();

            Console.WriteLine(debugText);
        }

        private void LogInformation(LogMessage message)
        {
            var debugText = new StringBuilder("==== INFORMATION ====")
                .Append(Environment.NewLine)
                .Append($"Time: {GetCurrentDateTime()}")
                .Append(Environment.NewLine)
                .Append($"Message: {message.Message}")
                .ToString();

            Console.WriteLine(debugText);

        }

        private string GetCurrentDateTime()
        {
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        #endregion

        #endregion

        #region Events

        private Task LogAsync(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Error:
                case LogSeverity.Critical:
                    LogError(message);
                    break;

                case LogSeverity.Debug:
                    LogDebugging(message);
                    break;

                default:
                    LogInformation(message);
                    break;
            }

            return Task.CompletedTask;
        }

        #endregion

    }
}
