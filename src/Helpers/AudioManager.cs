using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;

using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using YoutubeDLSharp.Metadata;

using Discord;
using Discord.Audio;
using Discord.WebSocket;

namespace AivaptDotNet.Helpers
{
    public class AudioManager
    {
        #region Constructors
        public AudioManager()
        {
            AudioQueue = new ObservableCollection<FormatData>();
            AudioQueue.CollectionChanged += OnQueueChanged;
            CurrentAudio = null;
            CurrentAudioClient = null;

        }

        #endregion

        #region Fields and Properties

        private ObservableCollection<FormatData> AudioQueue;
        public FormatData CurrentAudio;
        public IAudioClient CurrentAudioClient;
        public IVoiceChannel CurrentVoiceChannel;
        private CancellationTokenSource CurrentCancellationTokenSource;

        #endregion

        #region Events

        private async void OnQueueChanged(object sender, NotifyCollectionChangedEventArgs e) 
        {
            if(e.Action == NotifyCollectionChangedAction.Add && CurrentAudio == null) // triggered whenever a song is added to the queue
            {
                if(e.NewItems.Count < 1 || AudioQueue.Count > 1) return;
                await OnQueueAdded();
            }
        }

        private async Task OnQueueAdded()
        {
            while(AudioQueue != null && AudioQueue.Count != 0) // as long as songs are in the queue, the background task will iterate over this list
            {
                FormatData format = AudioQueue[0];
                string audioStreamUrl = format.Url;

                if(audioStreamUrl != null && audioStreamUrl.Length > 1)
                {
                    CurrentAudio = format;
                    AudioQueue.RemoveAt(AudioQueue.Count - 1);
                    try
                    {
                        await SendAudio(CurrentAudioClient, CurrentAudio.Url);
                    }
                    catch(OperationCanceledException)
                    {
                        // send message to channel that song was skipped successfully
                    }
                    CurrentAudio = null;
                }
            }
        }

        #endregion

        #region Private Methods

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
                AudioQuality = 0,
                Format = "bestaudio/best"
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
                AudioQuality = 0,
                Format = "bestaudio/best"
            };
            try
            {
                var result = await ytdl.RunVideoDataFetch(url, overrideOptions: options);
                return result;
            }
            catch(Exception)
            {
                return null;
            }
        }

        private Process CreateStream(string audioPath)
        {
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
            CurrentCancellationTokenSource = new CancellationTokenSource();
            CancellationToken currentCancellationToken = CurrentCancellationTokenSource.Token;

            using(var ffmpeg = CreateStream(audioPath))
            using(var outputSource = ffmpeg.StandardOutput.BaseStream)
            using(var outStream = audioClient.CreatePCMStream(AudioApplication.Music, bufferMillis: 40960, packetLoss: 10))
            {
                try
                {
                    //TODO: own stream reader to interrupt copying and reading
                    await outputSource.CopyToAsync(outStream, 40960, currentCancellationToken);

                } 
                finally
                {
                    //TODO: end is always cut off
                    await outStream.FlushAsync();
                    CurrentCancellationTokenSource.Dispose();
                    CurrentCancellationTokenSource = null;
                }
            }
        }

        private SocketVoiceChannel GetUserVoiceChannel(IReadOnlyCollection<Discord.WebSocket.SocketVoiceChannel> channels, ulong userID)
        {
            foreach(var channel in channels)
            {
                foreach(var user in channel.Users)
                {
                    if(user.Id == userID)
                    {
                        return channel;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Public Methods

        public async Task JoinVoiceChannel(IVoiceChannel channel)
        {
            CurrentAudioClient = await channel.ConnectAsync();
            CurrentVoiceChannel = channel;
        }

        public async Task JoinVoiceChannel(SocketGuild guild, ulong userUD)
        {
            SocketVoiceChannel userChannel = GetUserVoiceChannel(guild.VoiceChannels, userUD);
            if(userChannel == null) return;

            CurrentAudioClient = await userChannel.ConnectAsync();
            CurrentVoiceChannel = userChannel;
        }

        public async void LeaveVoiceChannel()
        {
            if(CurrentVoiceChannel == null) return;
            await CurrentVoiceChannel.DisconnectAsync();
            CurrentAudioClient = null;
            CurrentVoiceChannel = null;
        }

        public bool PlayAudio(string url)
        {
            var result = GetAudioData(url).Result.Data;
            if(result != null)
            {
                AudioQueue.Add(result.Formats[0]);
                return true;
            }
            return false;
        }

        public void SkipAudio()
        {
            if(CurrentAudio != null && CurrentCancellationTokenSource != null)
            {
                CurrentCancellationTokenSource.Cancel();
            }
        }

        #endregion

    }
}