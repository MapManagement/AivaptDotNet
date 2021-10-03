using System;
using System.Threading.Tasks;
using System.IO;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using AivaptDotNet.Handlers;


namespace AivaptDotNet
{
    public class Program
    {
        //Source: https://docs.stillu.cc/guides/getting_started/first-bot.html
        private DiscordSocketClient _botClient;
        private CommandService _commands;

	    public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            _botClient = new DiscordSocketClient();
            _botClient.Log += Logging;

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false,
            });
            _commands.Log += Logging;
        }

	    public async Task MainAsync()
	    {
            string token = File.ReadAllText("token.txt");

            var commandHandler = new CommandHandler(_botClient, _commands);
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
