using System;
using System.Threading.Tasks;
using System.IO;

using Discord.Commands;
using Discord;

using AivaptDotNet.Helpers;


namespace AivaptDotNet.Modules 
{
    public class GeneralModule : ModuleBase<AivaptCommandContext>
    {
        [Command("test")]
        [Summary("Simple Test-Command")]
        public async Task TestCommand()
        {   
            await Context.Channel.SendMessageAsync("Test");
        }

        [Command("info")]
        [Summary("Information about the bot")]
        public async Task InfoCommand()
        {
            //await Context.Channel.SendMessageAsync("", false, SimpleEmbed.ErrorEmbed("Text"));

            OperatingSystem os = Environment.OSVersion;

            var botUser = Context.Client.GetUser(476002638169767936);
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle($"Information - {botUser.Username}");
            builder.AddField("Server OS", os.VersionString.ToString(), false);
            builder.AddField("Created at", botUser.CreatedAt.ToString("HH:mm | dd.MM.yyyy"), false);
            builder.AddField("User ID", botUser.Id, false);
            builder.WithThumbnailUrl(botUser.GetAvatarUrl());

            builder.WithColor(Color.Teal);
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}