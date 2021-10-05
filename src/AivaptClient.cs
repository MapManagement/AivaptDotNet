using Discord;
using Discord.WebSocket;
using Discord.Commands;


namespace AivaptDotNet
{
    public class AivaptClient : DiscordSocketClient
    {
        public SocketUser Profile;
        public SocketUser Creator;

        public AivaptClient()
        {
            Profile = this.GetUser(476002638169767936);
            Creator = this.GetUser(325936808338784268);
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