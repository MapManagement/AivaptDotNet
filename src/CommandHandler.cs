using System.Threading.Tasks;
using System.Reflection;
using System;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using AivaptDotNet.Helpers;


namespace AivaptDotNet.Handlers 
{
    public class CommandHandler {
        private readonly AivaptClient _botClient;
        private readonly CommandService _commandService;
        private readonly DbConnector _dbConnector;


        public CommandHandler(AivaptClient botClient, CommandService commandService, DbConnector dbConnector)
        {
            _botClient = botClient;
            _commandService = commandService;
            _dbConnector = dbConnector;
        }

        public async Task InitializeCommands()
        {
            _botClient.MessageReceived += HandleCommand;
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), services: null);
        }

        private async Task HandleCommand(SocketMessage msg) 
        {
            var message = msg as SocketUserMessage;
            if(message == null) return;

            int argPos = 0;

            if(!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_botClient.CurrentUser, ref argPos)) || message.Author.IsBot) return;

            var context = new AivaptCommandContext(_botClient, message, _dbConnector);

            await _commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null
            );
        }
    }
}