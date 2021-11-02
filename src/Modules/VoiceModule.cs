using System;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

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

        [Command("join", RunMode = RunMode.Async)]
        [Summary("Bot joins the voice channel")]
        public async Task JoinCommand(IVoiceChannel channel = null)
        {

            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("ERROR"); return; }

            Context.Client.CurrentVoiceChannel = channel;
            var audioClient = await channel.ConnectAsync();
            Context.Client.CurrentAudioClient = audioClient;
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Summary("Bot leaves the voice channel")]
        public async Task LeaveCommand()
        {
            var currentCHannel = Context.Client.CurrentVoiceChannel;
            if(currentCHannel == null) return;
            await currentCHannel.DisconnectAsync();
        }

        [Command("play", RunMode = RunMode.Async)]
        [Summary("Bot plays given audio")]
        public async Task PlayCommand(string audioPath)
        {
            if(Context.Client.CurrentAudioClient == null) return;

            VideoData audioData = GetAudioData(audioPath).Result.Data;
            if(audioData == null)
            {
                await Context.Channel.SendMessageAsync("The specified link didn't lead to any valid source.");
                return;
            }
            FormatData format = audioData.Formats[0];
            string audioStreamUrl = format.Url;

            if(audioStreamUrl != null && audioStreamUrl.Length > 1)
            {
                await SendAudio(Context.Client.CurrentAudioClient, audioStreamUrl);
            }

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

        private async Task<string> DownloadMp3(string url)
        {
            YoutubeDL ytdl = new YoutubeDL();
            
            string cwd = Directory.GetCurrentDirectory();
            string resourcePath = "/src/Resources/Audio/";

            ytdl.OutputFolder = cwd + resourcePath;
            ytdl.FFmpegPath = "/usr/bin/ffmpeg";
            ytdl.YoutubeDLPath = "/usr/local/bin/youtube-dl";

            OptionSet options = new OptionSet()
            {
                AudioFormat = AudioConversionFormat.Mp3,
                RestrictFilenames = true,
            };
            try
            {
                var result = await ytdl.RunAudioDownload(url, AudioConversionFormat.Mp3, overrideOptions: options);
                return result.Data;
            }
            catch(Exception)
            {
                return "";
            }
        }

        private async Task<RunResult<VideoData>> GetAudioData(string url)
        {
            YoutubeDL ytdl = new YoutubeDL();
            
            string cwd = Directory.GetCurrentDirectory();
            string resourcePath = "/src/Resources/Audio/";

            ytdl.OutputFolder = cwd + resourcePath;
            ytdl.FFmpegPath = "/usr/bin/ffmpeg";
            ytdl.YoutubeDLPath = "/usr/local/bin/youtube-dl";

            OptionSet options = new OptionSet()
            {
                AudioFormat = AudioConversionFormat.Mp3,
                RestrictFilenames = true,
            };
            try
            {
                var result = await ytdl.RunVideoDataFetch(url);
                return result;
            }
            catch(Exception)
            {
                return null;
            }
        }

        /*
        https://stackoverflow.com/questions/56026466/streaming-mp3-to-discord-net-2-0-audio-is-super-fast-chipmunk-ideas
        https://stackoverflow.com/questions/184683/play-audio-from-a-stream-using-c-sharp
        https://docs.stillu.cc/api/Discord.Audio.Streams.InputStream.html
        */

        private Process CreateStream(string audioPath) {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName="ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{audioPath}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
            return process;
        }

        private async Task SendAudio(IAudioClient audioClient, string audioPath)
        {
            using(var ffmpeg = CreateStream(audioPath))
            using(var output = ffmpeg.StandardOutput.BaseStream)
            using(var stream = audioClient.CreatePCMStream(AudioApplication.Music))
            {
                try { await output.CopyToAsync(stream); }
                finally { await stream.FlushAsync(); }
            }
        }
    }
}