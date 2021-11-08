using System;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized ;
using System.Net;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
using YoutubeDLSharp.Metadata;

using Discord.Audio;


//TODO: improve audio quality
namespace AivaptDotNet.Helpers
{
    public class AudioManager
    {
        public ObservableCollection<FormatData> AudioQueue;
        public FormatData CurrentAudio;
        public IAudioClient AudioClient;


        public AudioManager(IAudioClient audioClient)
        {
            AudioQueue = new ObservableCollection<FormatData>();
            AudioQueue.CollectionChanged += OnQueueChanged;
            CurrentAudio = null;
            AudioClient = audioClient;

        }

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
                    await SendAudio(AudioClient, CurrentAudio.Url);
                    CurrentAudio = null;
                }
            }
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

        public async Task<RunResult<VideoData>> GetAudioData(string url)
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

        /*
        https://stackoverflow.com/questions/56026466/streaming-mp3-to-discord-net-2-0-audio-is-super-fast-chipmunk-ideas
        https://stackoverflow.com/questions/184683/play-audio-from-a-stream-using-c-sharp
        https://docs.stillu.cc/api/Discord.Audio.Streams.InputStream.html
        */

        public Process CreateStream(string audioPath) {
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
            using(var stream = audioClient.CreatePCMStream(AudioApplication.Music, bufferMillis: 10000))
            {
                try { await output.CopyToAsync(stream); }
                finally { await stream.FlushAsync(); }
            }
        }
    }
}