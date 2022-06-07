using System;
using System.Threading.Tasks;

using Discord;
using Discord.Interactions;

namespace AivaptDotNet.Modules 
{
    public class GeneralModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("test", "Check the bot connection.")]
        public async Task TestCommand()
        {   
            await Context.Channel.SendMessageAsync("Test");
        }

        [SlashCommand("info", "Get general information about the bot.")]
        public async Task InfoCommand()
        {
            OperatingSystem os = Environment.OSVersion;

            var botUser = await Context.Client.GetUserAsync(476002638169767936);
            
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle($"Information - {botUser.Username}")
                .AddField("Server OS", os.VersionString.ToString(), false)
                .AddField("Created at", botUser.CreatedAt.ToString("HH:mm | dd.MM.yyyy"), false)
                .AddField("User ID", botUser.Id, false)
                .WithThumbnailUrl(botUser.GetAvatarUrl())
                .WithColor(Color.Teal);

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}