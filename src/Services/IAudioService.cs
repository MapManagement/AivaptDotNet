using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace AivaptDotNet.Services
{
    public interface IAudioService
    {
        Task ContinueAudioAsync();
        Task JoinAsync(IVoiceChannel voiceChannel);
        Task LeaveAsync(IGuild guild);
        Task<string> PlayAudioAsync(string url, CommandContext context);
        Task SkipAudioAsync();
        Task StopAudioAsync();
    }
}