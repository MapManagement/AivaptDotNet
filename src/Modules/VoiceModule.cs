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

            Context.Client.CurrentVoiceChannel = channel;
            var audioClient = await channel.ConnectAsync();
            Context.Client.CurrentAudioClient = Context.Client.ClientAudioManager.AudioClient = audioClient;
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Summary("Bot leaves the voice channel")]
        public async Task LeaveCommand()
        {
            var currentChannel = Context.Client.CurrentVoiceChannel;
            if(currentChannel == null) return;
            await currentChannel.DisconnectAsync();
            Context.Client.CurrentAudioClient = Context.Client.ClientAudioManager.AudioClient = null;
        }

        [Command("play", RunMode = RunMode.Async)]
        [Summary("Bot plays given audio")]
        public async Task PlayCommand(string audioPath)
        {
            if(Context.Client.CurrentAudioClient == null) return;

            VideoData audioData = Context.Client.ClientAudioManager.GetAudioData(audioPath).Result.Data;
            if(audioData == null)
            {
                await Context.Channel.SendMessageAsync("The specified link didn't lead to any valid source.");
                return;
            }
            FormatData format = audioData.Formats[0];

            Context.Client.ClientAudioManager.AudioQueue.Add(format);

            /*string audioStreamUrl = format.Url;

            if(audioStreamUrl != null && audioStreamUrl.Length > 1)
            {
                await SendAudio(Context.Client.CurrentAudioClient, audioStreamUrl);

            }*/

            /*string filePath = DownloadMp3(audioPath).Result;

            if(filePath.Length > 0)
            {
            await SendAudio(Context.Client.CurrentAudioClient, filePath);
            }
            else
            {
                await Context.Channel.SendMessageAsync("The specified link didn't lead to any valid source.");
            }*/
        }

         [Command("skip", RunMode = RunMode.Async)]
        [Summary("Skips current song")]
        public async Task SkipCommand()
        {
            Context.Client.ClientAudioManager.SkipAudio();
        }

        #endregion

    }
}