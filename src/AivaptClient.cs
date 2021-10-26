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

    }

    public class AivaptCommandContext : SocketCommandContext
    {
        public new AivaptClient Client;
        public DbConnector _dbConnector;
        public AivaptCommandContext(AivaptClient client, SocketUserMessage message, DbConnector dbConnector) : base(client, message)
        {
            Client = client as AivaptClient;
            _dbConnector = dbConnector;
        }
    }
}