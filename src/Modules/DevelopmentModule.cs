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
            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>();
            fields.Add(new EmbedFieldBuilder{Name = "ID", Value = repo.ID ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Created at", Value = repo.CreatedAt.ToString("HH:mm | dd.MM.yyyy") ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Last updated", Value = repo.UpdatedAt.ToString("HH:mm | dd.MM.yyyy") ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Languages", Value = repo.Language ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Wacthers", Value = repo.Watchers ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Stars", Value = repo.Stars ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Open issues", Value = repo.OpenIssues ,IsInline = true});

            EmbedBuilder builder =  SimpleEmbed.FieldsEmbed(repo.Name, "General Information", fields);
            builder.WithUrl(repo.URL);
            builder.WithColor(Color.Teal);

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