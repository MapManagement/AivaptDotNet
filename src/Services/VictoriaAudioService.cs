using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
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

        public async Task<string> PlayAudioAsync(string url, SocketInteractionContext context)
        {
            var player = _lavaNode.GetPlayer(context.Guild);

            if (!_lavaNode.HasPlayer(context.Guild))
            {
                await JoinAsync(((IVoiceState)context.User).VoiceChannel);
            }

            if (player == null)
                return "Aivapt is currently not connected to any voice channel.";

            var searchRepsonse = await GetSearchResponse(url) as SearchResponse?;

            if (searchRepsonse == null)
                return "Couldn't find that song!";

            var trackResponse = await AddTrackToQueue(searchRepsonse.Value, player);
			var track = searchRepsonse.Value.Tracks.First();

            if (trackResponse == TrackStatusResponse.AddingToQueue)
                return $"Added {track.Title} by {track.Author} to the queue.";
            else
                return $"Playing {track.Title} by {track.Author}.";
        }

        public async Task<string> SkipAudioAsync(SocketInteractionContext context)
        {
            var player = _lavaNode.GetPlayer(context.Guild);

            if (player == null)
                return "Aivapt is currently not connected to any voice channel.";

            if (player.Queue.Count == 0)
				return "There are no songs in the queue.";
            
            var currentTrack = player.Track;
            await player.SkipAsync();

            return $"Skipped {currentTrack.Title} by {currentTrack.Author}.";
        }

        public async Task<string> PauseAudioAsync(SocketInteractionContext context)
        {
            var player = _lavaNode.GetPlayer(context.Guild);

            if (player == null)
                return "Aivapt is currently not connected to any voice channel.";

            if (player.PlayerState == PlayerState.Playing)
            {
                await player.PauseAsync();

                return "Paused.";
            }
            else
            {
                return "Aivapt is currently not playing any music.";
            }
        }

		public async Task<string> StopAudioAsync(SocketInteractionContext context)
		{
			var player = _lavaNode.GetPlayer(context.Guild);

            if (player == null)
                return "Aivapt is currently not connected to any voice channel.";

            if (player.PlayerState == PlayerState.Playing)
            {
                await player.StopAsync();

                return "Stopped.";
            }
            else
            {
                return "Aivapt is currently not playing any music.";
            }

		}

        public async Task<string> ContinueAudioAsync(SocketInteractionContext context)
        {

            var player = _lavaNode.GetPlayer(context.Guild);

            if (player == null)
                return "Aivapt is currently not connected to any voice channel.";

            if (player.PlayerState == PlayerState.Paused || player.PlayerState == PlayerState.Stopped)
            {
                await player.ResumeAsync();
                return "Continuing...";
            }
            else
            {
                return "Aivapt is currently not paused.";
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
