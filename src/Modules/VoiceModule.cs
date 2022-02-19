using System.Threading.Tasks;

using AivaptDotNet.Services;

using Discord;
using Discord.Commands;

using Victoria;

namespace AivaptDotNet.Modules 
{
    public class VoiceModule : ModuleBase<CommandContext>
    {
        #region Fields

        public IAudioService AudioService { get; set; }

        #endregion

        #region Commands

        [Command("join")]
        public async Task JoinCommand()
        {
            var userChannel = ((IVoiceState)Context.User).VoiceChannel;

            if(userChannel != null)
            {
                await AudioService.JoinAsync(userChannel);
            }
            else
            {
                await ReplyAsync("You're not connected to a voice channel!");
            }
        }

        [Command("leave")]
        public async Task LeaveCommand()
        {
            await AudioService.LeaveAsync(Context.Guild);
        }

        [Command("play")]
        public async Task PlayCommand(string url)
        {
            var userChannel = ((IVoiceState)Context.User).VoiceChannel;
            if(userChannel == null)
                await ReplyAsync("You're not connected to a voice channel.");

            var message = await AudioService.PlayAudioAsync(url, Context);
            await ReplyAsync(message);
        }

        [Command("skip")]
        public async Task SkipCommand()
        {
            var message = await AudioService.SkipAudioAsync(Context);
            await ReplyAsync(message);
        }

        [Command("stop")]
        public async Task StopCommand()
        {
            var message = await AudioService.StopAudioAsync(Context);
            await ReplyAsync(message);
        }

        [Command("continue")]
        public async Task ContinueCommand()
        {
            var message = await AudioService.ContinueAudioAsync(Context);
            await ReplyAsync(message); 
        }

        #endregion
    }
}