using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System;

using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using AivaptDotNet.Helpers;
using AivaptDotNet.Services;


namespace AivaptDotNet.Handlers
{
    public class CommandHandler
    {
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _botClient;
        private readonly CommandService _commandService;
        private readonly DatabaseService _dbService;

        public CommandHandler(IServiceProvider services)
        {
            _services = services;
            _botClient = services.GetRequiredService<DiscordSocketClient>();
            _commandService = services.GetRequiredService<CommandService>();
            _dbService = services.GetRequiredService<DatabaseService>();
        }

        public async Task InitializeCommands()
        {
            _botClient.MessageReceived += HandleCommand;
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), services: _services);
        }

        private async Task HandleCommand(SocketMessage msg) 
        {
            var message = msg as SocketUserMessage;
            if(message == null) return;

            int argPos = 0;

            if(!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_botClient.CurrentUser, ref argPos)) || message.Author.IsBot) return;

            
            var context = new CommandContext(_botClient, message);

            try 
            {
            var commandTask = await _commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services
            );

            // If the "normal" command is not available, the handler will check if a database command exists. If so, that command will be executed.
            if(!commandTask.IsSuccess)
            {
                await OnSimpleCommand(message);
            }

            }
            catch(Exception e)
            {
                Embed errorEmbed = SimpleEmbed.ErrorEmbed(e.InnerException.ToString().Substring(0, 256));
                await context.Channel.SendMessageAsync("", false, errorEmbed);
            }
           
        }

        private async Task OnSimpleCommand(SocketMessage message)
        {
            var msg = message.ToString();
            if(CheckCommandExistence(msg.Remove(0,1)))
            {
                string sql = "select * from simple_command where name = @COMMAND_NAME";
                var param = new Dictionary<string, object>();
                param.Add("@COMMAND_NAME", msg.Remove(0,1));

                using var result = _dbService.ExecuteSelect(sql, param);

                if(!result.HasRows) await message.Channel.SendMessageAsync("Error");
                result.Read();

                string title = result.GetString("title");
                string text = result.GetString("command_text");
                Color color = SimpleConverter.HexToColor(result.GetString("color"));

                EmbedBuilder builder = SimpleEmbed.MinimalEmbed(title, text);
                builder.WithColor(color);

                await message.Channel.SendMessageAsync("", false, builder.Build());
            }
            
        }

        private bool CheckCommandExistence(string name)
        {   
            string sql = "select 1 from simple_command where name = @COMMAND_NAME";
            var param = new Dictionary<string, object>();
            param.Add("@COMMAND_NAME", name);

            using var result = _dbService.ExecuteSelect(sql, param);
            if(result.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}