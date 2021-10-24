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

	    public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            _botClient = new AivaptClient();
            _botClient.Log += Logging;
            //_botClient.MessageReceived += OnMessage;

            string connectionString = File.ReadAllText("src/sql_connection_string.txt");
            _dbConnection = new DbConnector(new MySqlConnection(connectionString));

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Error,
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
    }   
}
