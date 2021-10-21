using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Audio;

using AivaptDotNet.Helpers;


namespace AivaptDotNet
{
    public class AivaptClient : DiscordSocketClient
    {
        public IVoiceChannel CurrentVoiceChannel;
        public IAudioClient CurrentAudioClient;

        public AivaptClient()
        {

        }

    }

    public class AivaptCommandContext : SocketCommandContext
    {
        public AivaptClient Client;
        public SocketUserMessage Message;
        public DbConnector _dbConnector;
        public AivaptCommandContext(DiscordSocketClient client, SocketUserMessage message, DbConnector dbConnector) : base(client, message)
        {
            Client = client as AivaptClient;
            Message = message;
            _dbConnector = dbConnector;
        }
    }
}