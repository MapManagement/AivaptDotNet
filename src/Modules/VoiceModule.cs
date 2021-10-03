using System;
using System.Threading.Tasks;
using System.IO;

using Discord;
using Discord.Audio;
using Discord.Commands;


namespace AivaptDotNet.Modules 
{
    public class VoiceModules : ModuleBase<SocketCommandContext>
    {
        [Command("join", RunMode = RunMode.Async)]
        [Summary("Bot joins the voice channel")]
        public async Task JoinCommand(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("ERROR"); return; }
            var audioClient = await channel.ConnectAsync();
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Summary("Bot leaves the voice channel")]
        public async Task LeaveCommand(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("ERROR"); return; }
            await channel.DisconnectAsync();
        }
    }
}