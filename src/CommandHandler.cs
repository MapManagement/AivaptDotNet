using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
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

            try 
            {
            var commandTask = await _commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null
            );

            // If the "normal" command is not available, the handler will check if a database command exists. If so, that command will be executed.
            if(!commandTask.IsSuccess)
            {
                await OnSimpleCommand(message);
            }

            }
            catch(Exception e)
            {
                Embed errorEmbed = SimpleEmbed.ErrorEmbed(e.InnerException.ToString()); //TODO: change error text
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

                using var result = _dbConnector.ExecuteSelect(sql, param);

                if(!result.HasRows) await message.Channel.SendMessageAsync("Error");
                result.Read();

                EmbedBuilder builder = new EmbedBuilder();
                
                builder.WithTitle(result.GetString("title"));
                var text = result.GetString("command_text");
                builder.AddField("Text", text); //TODO: find solution for field name

                Color color = ConverterHelper.HexToColor(result.GetString("color"));
                builder.WithColor(color);

                await message.Channel.SendMessageAsync("", false, builder.Build());
            }
            
        }

        private bool CheckCommandExistence(string name)
        {   
            string sql = "select 1 from simple_command where name = @COMMAND_NAME";
            var param = new Dictionary<string, object>();
            param.Add("@COMMAND_NAME", name);

            using var result = _dbConnector.ExecuteSelect(sql, param);
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