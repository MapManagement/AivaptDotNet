using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace AivaptDotNet.Services
{
    public class AudaptAudioService : IAudioService
    {
        public Task<string> ContinueAudioAsync(CommandContext context)
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

        public Task<string> SkipAudioAsync(CommandContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> StopAudioAsync(CommandContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}