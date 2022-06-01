using System.Threading.Tasks;
using Discord;
using Discord.Interactions;

namespace AivaptDotNet.Services
{
    public class AudaptAudioService : IAudioService
    {
        public Task<string> ContinueAudioAsync(SocketInteractionContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task JoinAsync(IVoiceChannel voiceChannel)
        {
            throw new System.NotImplementedException();
        }

        public Task LeaveAsync(IGuild guild)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> PlayAudioAsync(string url, SocketInteractionContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> SkipAudioAsync(SocketInteractionContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> StopAudioAsync(SocketInteractionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}