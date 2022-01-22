using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace AivaptDotNet.Services
{
    public class AudaptAudioService : IAudioService
    {
        public Task ContinueAudioAsync()
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

        public Task<string> PlayAudioAsync(string url, CommandContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task SkipAudioAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task StopAudioAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}