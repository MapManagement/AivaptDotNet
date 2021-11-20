using System;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized ;
using System.Collections.Generic;

using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using YoutubeDLSharp.Metadata;

using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Audio.Streams;

namespace AivaptDotNet.Modules 
{
    public class VoiceModules : ModuleBase<AivaptCommandContext>
    {

        #region Commands

        [Command("join", RunMode = RunMode.Async)]
        [Summary("Bot joins the voice channel")]
        public async Task JoinCommand(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("ERROR"); return; }

            Context.Client.ClientAudioManager.JoinVoiceChannel(channel);
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Summary("Bot leaves the voice channel")]
        public async Task LeaveCommand()
        {
            Context.Client.ClientAudioManager.LeaveVoiceChannel();
        }

        [Command("play", RunMode = RunMode.Async)]
        [Summary("Bot plays given audio")]
        public async Task PlayCommand(string audioPath)
        {
            if(Context.Client.ClientAudioManager.CurrentAudioClient == null) return;

            bool success = Context.Client.ClientAudioManager.PlayAudio(audioPath);
            if(success)
            {
                await Context.Channel.SendMessageAsync("The specified link didn't lead to any valid source.");
            }
        }

        [Command("skip", RunMode = RunMode.Async)]
        [Summary("Skips current audio")]
        public async Task SkipCommand()
        {
            Context.Client.ClientAudioManager.SkipAudio();
            await Context.Channel.SendMessageAsync("Skipping audio...");
        }

        [Command("stop", RunMode = RunMode.Async)]
        [Summary("Stops current audio")]
        public async Task StopCommand()
        {
            //TODO: add stop method
            await Context.Channel.SendMessageAsync("Stopping audio...");
        }

        [Command("continue", RunMode = RunMode.Async)]
        [Summary("Continues playing audio")]
        public async Task ContinueCommand()
        {
            //TODO: continue stop method
            await Context.Channel.SendMessageAsync("Continuing...");
        }

        #endregion

    }
}