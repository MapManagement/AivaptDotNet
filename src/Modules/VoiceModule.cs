using System.Threading.Tasks;

using AivaptDotNet.Services;

using Discord;
using Discord.Interactions;
using Victoria;

namespace AivaptDotNet.Modules
{
    [Group("audio", "Any voice/audio related commands")]
    public class VoiceModule : InteractionModuleBase<SocketInteractionContext>
    {
        #region Fields

        public IAudioService AudioService { get; set; }

        #endregion

        #region Commands

        [SlashCommand("join", "Let the bot join your current voice channel.")]
        public async Task JoinCommand()
        {
            var userChannel = ((IVoiceState)Context.User).VoiceChannel;

            if (userChannel != null)
            {
                await AudioService.JoinAsync(userChannel);
            }
            else
            {
                await RespondAsync("You're not connected to a voice channel!");
            }
        }

        [SlashCommand("leave", "Disconnect the bot from a voice channel.")]
        public async Task LeaveCommand()
        {
            await AudioService.LeaveAsync(Context.Guild);
        }

        [SlashCommand("play", "Play some audio.")]
        public async Task PlayCommand(string url)
        {
            var userChannel = ((IVoiceState)Context.User).VoiceChannel;

            if (userChannel == null)
                await RespondAsync("You're not connected to a voice channel.");

            var message = await AudioService.PlayAudioAsync(url, Context);
            await RespondAsync(message);
        }

        [SlashCommand("skip", "Skip the current audio.")]
        public async Task SkipCommand()
        {
            var message = await AudioService.SkipAudioAsync(Context);
            await RespondAsync(message);
        }

        [SlashCommand("stop", "Stop the current audio.")]
        public async Task StopCommand()
        {
            var message = await AudioService.StopAudioAsync(Context);
            await RespondAsync(message);
        }

        [SlashCommand("continue", "Continue playing the current aduio.")]
        public async Task ContinueCommand()
        {
            var message = await AudioService.ContinueAudioAsync(Context);
            await RespondAsync(message);
        }

        #endregion
    }
}
