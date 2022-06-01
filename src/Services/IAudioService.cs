using System.Threading.Tasks;

using Discord;
using Discord.Interactions;

namespace AivaptDotNet.Services
{
    public interface IAudioService
    {
        Task<string> ContinueAudioAsync(SocketInteractionContext context);
        Task JoinAsync(IVoiceChannel voiceChannel);
        Task LeaveAsync(IGuild guild);
        Task<string> PlayAudioAsync(string url, SocketInteractionContext context);
        Task<string> SkipAudioAsync(SocketInteractionContext context);
        Task<string> StopAudioAsync(SocketInteractionContext context);
    }
}