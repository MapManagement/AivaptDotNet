using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

namespace AivaptDotNet.Helpers
{
    public static class ResourceManager 
    {
        public static Emoji GreenCircleEmoji = new Emoji("🟢");
        public static Emoji RedCircleEmoji = new Emoji("🔴");
        public static Emoji CheckEmoji = new Emoji("☑️");
    }

}