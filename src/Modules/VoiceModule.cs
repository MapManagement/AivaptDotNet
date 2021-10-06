using System;
using System.Threading.Tasks;
using System.IO;

using Discord;
using Discord.Audio;
using Discord.Commands;


namespace AivaptDotNet.Modules 
{
    public class VoiceModules : ModuleBase<AivaptCommandContext>
    {

        [Command("join", RunMode = RunMode.Async)]
        [Summary("Bot joins the voice channel")]
        public async Task JoinCommand(IVoiceChannel channel = null)
        {

            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("ERROR"); return; }

            Context.Client.CurrentVoiceChannel = channel;
            var audioClient = await channel.ConnectAsync();
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Summary("Bot leaves the voice channel")]
        public async Task LeaveCommand()
        {
            var currentCHannel = Context.Client.CurrentVoiceChannel;
            if(currentCHannel == null) return;
            await currentCHannel.DisconnectAsync();
        }
    }
}