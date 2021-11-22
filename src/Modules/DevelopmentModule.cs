using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

using AivaptDotNet.Helpers;
using AivaptDotNet.AivaptClases.Json;

namespace AivaptDotNet.Modules 
{
    [Group("dev")]
    public class DevelopmentModule: ModuleBase<AivaptCommandContext>
    {

        [Command("latest")]
        [Summary("Sends information about the latest commit.")]
        public async Task LatestCommand()
        {
            return;
        }

        [Command("info")]
        [Summary("Sends general information about the project.")]
        public async Task InfoCommand()
        {
            List<string> headersList = new List<string>() { "application/vnd.github.v3+json" };
            string response = await HttpRequests.SimpleGetRequest(
                "https://api.github.com/repos/mapmanagement/AivaptDotNet",
                headersList,
                ".NET Foundation Repository Reporter"
                );

            Repository repo = JsonSerializer.Deserialize<Repository>(response);
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("ID", repo.ID.ToString());
            fields.Add("Language", repo.Language);
            fields.Add("Watchers", repo.Watchers.ToString());
            fields.Add("Stars", repo.Stars.ToString());
            fields.Add("Open Issues", repo.OpenIssues.ToString());
            EmbedBuilder builder =  SimpleEmbed.FieldsEmbed("Development Information", repo.Name, fields);

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [Command("release")]
        [Summary("Sends the latest release of the bot.")]
        public async Task ReleaseCommand()
        {
            return;
        }

        [Command("issues")]
        [Summary("Sends issues that are currently under development.")]
        public async Task IssuesCommand()
        {
            return;
        }
    }
}