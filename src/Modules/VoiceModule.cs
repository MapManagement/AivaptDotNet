using System;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

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
            if(Context.Client.CurrentAudioClient != null) return;
            CreateStream(audioPath);
            await SendAudio(Context.Client.CurrentAudioClient, audioPath);
        }

        /*
        https://stackoverflow.com/questions/56026466/streaming-mp3-to-discord-net-2-0-audio-is-super-fast-chipmunk-ideas
        https://stackoverflow.com/questions/184683/play-audio-from-a-stream-using-c-sharp
        https://docs.stillu.cc/api/Discord.Audio.Streams.InputStream.html
        */
        [Command("radio", RunMode = RunMode.Async)]
        public async Task RadioCommand(string channel)
        {
            /* @commands.command(help="Radio: ding | swr3 | hits")
                async def play_radio(self, ctx, sender: str):
                    voice_client = ctx.guild.voice_client
                    if voice_client.is_connected:
                        if voice_client.is_playing:
                            voice_client.stop()
                        radio_player = discord.FFmpegPCMAudio(source=self.radio[sender.lower()])
                        vol_radio = discord.PCMVolumeTransformer(radio_player, volume=0.1)
                        voice_client.play(vol_radio)
                        await ctx.send(f"Playing '{sender}'")
                    else:
                        await ctx.send("Not connected!")
            */
             var testChannel = "https://f131.rndfnk.com/ard/swr/swr3/live/mp3/128/stream.mp3?aggregator=web&sid=1zBr53lqvLjDErRmfBbF9hVtDCa&token=lb7CkZ0b0_rDZ5Xz7LySildLTM-qYq9hF77n0L0UXzE&tvf=OPOBrmrnqxZmMTMxLnJuZGZuay5jb20";
             var stream = new InputStream();
        }

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
            using(var discord = audioClient.CreateDirectPCMStream(AudioApplication.Mixed))
            {
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
            }
        }
    }
}