using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using AivaptDotNet.Handlers;
using AivaptDotNet.Helpers;


namespace AivaptDotNet
{
    public class Program
    {
        //Source: https://docs.stillu.cc/guides/getting_started/first-bot.html
        private AivaptClient _botClient;
        private CommandService _commands;
        private DbConnector _dbConnection;
        private List<string> _blacklist;

	    public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            _botClient = new AivaptClient();
            _botClient.Log += Logging;
            _botClient.MessageReceived += OnMessage;

            string connectionString = File.ReadAllText("src/sql_connection_string.txt");
            _dbConnection = new DbConnector(new MySqlConnection(connectionString));

            _blacklist = new List<string>() 
            {
                "!test", "!join", "!info", "!leave", "!play", "!radio"
            };

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false,
            });
            _commands.Log += Logging;
        }

	    public async Task MainAsync()
	    {
            string token = File.ReadAllText("src/token.txt");

            var commandHandler = new CommandHandler(_botClient, _commands, _dbConnection);
            await commandHandler.InitializeCommands();

            await _botClient.LoginAsync(TokenType.Bot, token);
            await _botClient.StartAsync();

            await Task.Delay(-1);
	    }

        private Task Logging(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task OnMessage(SocketMessage message)
        {
            var msg = message.ToString();
            if(msg.StartsWith("!"))
            {
                if(CheckCommandExistence(msg.Remove(0,1)) && !_blacklist.Contains(msg))
                {
                    string sql = "select * from simple_command where name = @COMMAND_NAME";
                    var param = new Dictionary<string, object>();
                    param.Add("@COMMAND_NAME", msg.Remove(0,1));

                    using var result = _dbConnection.ExecuteSelect(sql, param);

                    if(!result.HasRows) await message.Channel.SendMessageAsync("Error");
                    result.Read();

                    EmbedBuilder builder = new EmbedBuilder();
                    
                    builder.WithTitle(result.GetString("title"));
                    var text = result.GetString("command_text");
                    builder.AddField("Text", text); //TODO: find solution for field name

                    builder.WithColor(Color.Teal); //TODO: adding color from database
                    await message.Channel.SendMessageAsync("", false, builder.Build());
                }
            }
        }

        private bool CheckCommandExistence(string name)
        {   
            string sql = "select 1 from simple_command where name = @COMMAND_NAME";
            var param = new Dictionary<string, object>();
            param.Add("@COMMAND_NAME", name);

            using var result = _dbConnection.ExecuteSelect(sql, param);
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
