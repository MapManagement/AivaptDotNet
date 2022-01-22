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
        private readonly LavaNode _lavaNode;

        public VictoriaAudioService(LavaNode lavaNode)
        {
            _lavaNode = lavaNode;
        }

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
        }

        public async Task<string> PlayAudioAsync(string url, CommandContext context)
        {
            //TODO: implement play audio
            var player = _lavaNode.GetPlayer(context.Guild);

            if (!_lavaNode.HasPlayer(context.Guild))
            {
                await JoinAsync(((IVoiceState)context.User).VoiceChannel);
            }

            var urlResult = await _lavaNode.SearchAsync(SearchType.Direct, url);

            if (urlResult.Status == SearchStatus.LoadFailed || urlResult.Status == SearchStatus.NoMatches)
                return "Couldn't find any proper source.";

            if (urlResult.Tracks.Count < 1)
                return "Something went wrong...";

            player.Queue.Enqueue(urlResult.Tracks.FirstOrDefault());

            if (player.PlayerState is PlayerState.Playing or PlayerState.Paused)
            {
                return "Added source to queue.";
            }

            player.Queue.TryDequeue(out var lavaTrack);
            await player.PlayAsync(x =>
            {
                x.Track = lavaTrack;
                x.ShouldPause = false;
            });
            return "Playing song...";
        }

        public async Task SkipAudioAsync()
        {
            //TODO: implement skipd audio
        }

        public async Task StopAudioAsync()
        {
            //TODO: implement stop audio
        }

        public async Task ContinueAudioAsync()
        {
            //TODO: implement continue audio
        }
    }
}