using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Audio;

using AivaptDotNet.Helpers;
using AivaptDotNet.AivaptClases;


namespace AivaptDotNet
{
    public class AivaptClient : DiscordSocketClient
    {
        public IVoiceChannel CurrentVoiceChannel;
        public IAudioClient CurrentAudioClient;
        public AudioManager ClientAudioManager;

        public AivaptClient(DiscordSocketConfig config) : base(config)
        {
            ClientAudioManager = new AudioManager(CurrentAudioClient);
            //TODO: event when voice client updated
        }

        public AivaptClient(DiscordSocketConfig config, int statusString) : base(config)
        {
            ClientAudioManager = new AudioManager(CurrentAudioClient);;
            this.SetStatusAsync(0); // 0-5
        }

        public AivaptClient(DiscordSocketConfig config, int status, IActivity activity) : base(config)
        {
            ClientAudioManager = new AudioManager(CurrentAudioClient);;
            this.SetStatusAsync(0); // 0-5
            this.SetActivityAsync(activity);
        }

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