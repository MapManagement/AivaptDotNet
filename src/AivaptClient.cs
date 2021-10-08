using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Audio;


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
        public AivaptCommandContext(DiscordSocketClient client, SocketUserMessage message) : base(client, message)
        {
            Client = client as AivaptClient;
            Message = message;
        }
    }
}