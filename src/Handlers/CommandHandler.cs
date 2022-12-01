using System.Threading.Tasks;
using System.Reflection;
using System;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using AivaptDotNet.Services;
using AivaptDotNet.Helpers.General;
using AivaptDotNet.Helpers.DiscordClasses;
using AivaptDotNet.Helpers.Modules;


namespace AivaptDotNet.Handlers
{
    public class CommandHandler
    {
		#region Fields

        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _botClient;
        private readonly CommandService _commandService;
        private readonly DatabaseService _dbService;

		#endregion

		#region Constructor

        public CommandHandler(IServiceProvider services)
        {
            _services = services;
            _botClient = services.GetRequiredService<DiscordSocketClient>();
            _commandService = services.GetRequiredService<CommandService>();
            _dbService = services.GetRequiredService<DatabaseService>();
        }

		#endregion

		#region Methods

		#region Public

        public async Task InitializeCommands()
        {
            _botClient.MessageReceived += HandleCommand;
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), services: _services);
        }

		#endregion

		#region Private

        private async Task HandleCommand(SocketMessage msg) 
        {
            var message = msg as SocketUserMessage;
			int argPos = 0;

            if (message == null)
				return;

			bool messageHasNoPrefix = !(message.HasCharPrefix('!', ref argPos));
			bool hasMentionPrefix = message.HasMentionPrefix(_botClient.CurrentUser, ref argPos);
			bool isBot = message.Author.IsBot;

            if ((messageHasNoPrefix || hasMentionPrefix) || isBot)
				return;
            
            var context = new CommandContext(_botClient, message);

			await OnSimpleCommand(message);
        }

        private async Task OnSimpleCommand(SocketUserMessage message)
        {
            var msg = message.ToString();
			string commandName = msg.Remove(0, 1);
			bool commandExists = SimpleCommandHelper.IsCommandAvailable(_dbService, commandName);

			if (!commandExists)
				await message.ReplyAsync("This Simple Command is not available.");

			var simpleCommand = SimpleCommandHelper.GetSimpleCommand(_dbService, commandName);

			if (simpleCommand == null)
				await message.ReplyAsync("This Simple Command is not available.");

            Color color = Converters.HexToColor(simpleCommand.Color);

            EmbedBuilder builder = SimpleEmbed.MinimalEmbed(simpleCommand.Title, simpleCommand.Text);
            builder.WithColor(color);

            await message.Channel.SendMessageAsync("", false, builder.Build());
        }

		#endregion

		#endregion
    }
}
