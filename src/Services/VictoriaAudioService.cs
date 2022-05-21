using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Victoria;
using Victoria.Enums;
using Victoria.Responses.Search;

namespace AivaptDotNet.Services
{
    public class VictoriaAudioService : IAudioService
    {
        #region Enums

        enum TrackStatusResponse
        {
            AddingToQueue,
            Playing
        }   

        #endregion

        #region Fields

        private readonly LavaNode _lavaNode;

        #endregion

        #region Constructor

        public VictoriaAudioService(LavaNode lavaNode)
        {
            _lavaNode = lavaNode;
        }

        #endregion

        #region Methods

        #region Public Methods

        public async Task JoinAsync(IVoiceChannel voiceChannel)
        {
            await _lavaNode.JoinAsync(voiceChannel);
        }

        public async Task LeaveAsync(IGuild guild)
        {
            var player = _lavaNode.GetPlayer(guild);

            if (player.PlayerState == Victoria.Enums.PlayerState.Playing)
            {
                await player.StopAsync();
                await _lavaNode.LeaveAsync(player.VoiceChannel);
            }

            await _lavaNode.LeaveAsync(player.VoiceChannel);
        }

        public async Task<string> PlayAudioAsync(string url, CommandContext context)
        {
            var player = _lavaNode.GetPlayer(context.Guild);

            if (!_lavaNode.HasPlayer(context.Guild))
            {
                await JoinAsync(((IVoiceState)context.User).VoiceChannel);
            }

            if (player == null)
                return "Aivapt is currently not connected to any voice channel.";

            var searchRepsonse = await GetSearchResponse(url);

            if(searchRepsonse == null)
                return "Couldn't find that song!";

            var trackResponse = await AddTrackToQueue((SearchResponse)searchRepsonse, player);

            if (trackResponse == TrackStatusResponse.AddingToQueue)
                return "Added song to queue.";
            else
                return "Playing...";
        }

        public async Task<string> SkipAudioAsync(CommandContext context)
        {
            var player = _lavaNode.GetPlayer(context.Guild);

            if(player == null)
                return "Aivapt is currently not connected to any voice channel.";

            if(player.Queue.Count > 1)
            {
                var currentAudio = player.Track;
                await player.SkipAsync();
                return $"Skipped Audio: {currentAudio.Title}.";
            }
            else
            {
                return "No songs can be skipped.";
            }
        }

        public async Task<string> StopAudioAsync(CommandContext context)
        {
            var player = _lavaNode.GetPlayer(context.Guild);

            if(player == null)
                return "Aivapt is currently not connected to any voice channel.";

            if(player.PlayerState == PlayerState.Playing)
            {
                await player.PauseAsync();
                return "Stopped playing.";
            }
            else
            {
                return "Aivapt is currently not playing any music.";
            }
        }

        public async Task<string> ContinueAudioAsync(CommandContext context)
        {

            var player = _lavaNode.GetPlayer(context.Guild);

            if(player == null)
                return "Aivapt is currently not connected to any voice channel.";

            if(player.PlayerState == PlayerState.Paused)
            {
                await player.ResumeAsync();
                return "Continuing...";
            }
            else
            {
                return "Aivapt is currently not stopped.";
            }
        }

        #endregion

        #region Private Methods

        private async Task<SearchResponse?> GetSearchResponse(string url)
        {
            var urlResult = await _lavaNode.SearchAsync(SearchType.Direct, url);

            if (urlResult.Status == SearchStatus.LoadFailed ||
                urlResult.Status == SearchStatus.NoMatches  ||
                urlResult.Tracks.Count < 1)
                return null;

            return urlResult;
        }

        private async Task<TrackStatusResponse> AddTrackToQueue(SearchResponse searchResponse, LavaPlayer player)
        {
            player.Queue.Enqueue(searchResponse.Tracks.FirstOrDefault());

            if (player.PlayerState is PlayerState.Playing or PlayerState.Paused)
            {
                return TrackStatusResponse.AddingToQueue;
            }

            player.Queue.TryDequeue(out var lavaTrack);

            await player.PlayAsync(x =>
            {
                x.Track = lavaTrack;
                x.ShouldPause = false;
            });

            return TrackStatusResponse.Playing;
        }

        #endregion

        #endregion
    }
}