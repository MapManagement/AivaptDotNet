using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace AivaptDotNet.Services
{
    public interface IAudioService
    {
        Task<string> ContinueAudioAsync(CommandContext context);
        Task JoinAsync(IVoiceChannel voiceChannel);
        Task LeaveAsync(IGuild guild);
        Task<string> PlayAudioAsync(string url, CommandContext context);
        Task<string> SkipAudioAsync(CommandContext context);
        Task<string> StopAudioAsync(CommandContext context);
    }
}