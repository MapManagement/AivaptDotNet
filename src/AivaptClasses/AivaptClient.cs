using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Audio;

using AivaptDotNet.Helpers;
using AivaptDotNet.Database;


namespace AivaptDotNet
{
    public class AivaptClient : DiscordSocketClient
    {
        #region Constructors

        public AivaptClient(DiscordSocketConfig config) : base(config)
        {
            ClientAudioManager = new AudioManager();
            VoiceServerUpdated += OnVoiceServerUpdated;
        }

        public AivaptClient(DiscordSocketConfig config, int statusString) : base(config)
        {
            ClientAudioManager = new AudioManager();
            VoiceServerUpdated += OnVoiceServerUpdated;
            this.SetStatusAsync(0); // 0-5
        }

        public AivaptClient(DiscordSocketConfig config, int status, IActivity activity) : base(config)
        {
            ClientAudioManager = new AudioManager();
            VoiceServerUpdated += OnVoiceServerUpdated;
            this.SetStatusAsync(0); // 0-5
            this.SetActivityAsync(activity);
        }

        #endregion

        #region Fields and Properties

        public AudioManager ClientAudioManager;

        #endregion

        #region Events

        //TODO: only fire when bot connects -> own events are needed
        private Task OnVoiceServerUpdated(SocketVoiceServer voiceServer)
        {
            return Task.CompletedTask;
        }

        #endregion


    }

    public class AivaptCommandContext : SocketCommandContext
    {
        #region Constructor

        public AivaptCommandContext(AivaptClient client, SocketUserMessage message, Connector dbConnector) : base(client, message)
        {
            Client = client as AivaptClient;
            _dbConnector = dbConnector;
        }

        #endregion

        #region Fields and Properties

        public new AivaptClient Client;
        public Connector _dbConnector;

        #endregion
        
    }
}