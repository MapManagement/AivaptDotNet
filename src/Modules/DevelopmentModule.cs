using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http;

using Discord;

using AivaptDotNet.Helpers.General;
using AivaptDotNet.DataClasses;
using Discord.Interactions;

namespace AivaptDotNet.Modules 
{
    [Group("dev", "Check the developtment status of the AivaptDotNet GitHub repository")]
    public class DevelopmentModule: InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("latest", "Get the latest changes.")]
        public async Task LatestCommand()
        {
            List<string> headersList = new List<string>() { "application/vnd.github.v3+json" };
            string response = await HttpRequests.SimpleGetRequest(
                "https://api.github.com/repos/mapmanagement/AivaptDotNet/commits?page=1&per_page=1",
                headersList,
                ".NET Foundation Repository Reporter"
            );

            List<Commit> commitList = JsonSerializer.Deserialize<List<Commit>>(response);
            Commit commit = commitList[0];
            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>();
            fields.Add(new EmbedFieldBuilder{Name = "Author", Value = commit.Author.Login, IsInline = false});
            fields.Add(new EmbedFieldBuilder{Name = "Message", Value = commit.Details.Message, IsInline = false});

            EmbedBuilder builder =  SimpleEmbed.FieldsEmbed($"{commit.SHA.Substring(0,8)}", fields);
            builder.WithUrl(commit.URL);
            builder.WithThumbnailUrl(commit.Author.AvatarUrl);
            builder.WithColor(Color.Teal);
            builder.WithFooter("Latest Commit");

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [SlashCommand("info", "Get general information about the repository.")]
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
            fields.Add(new EmbedFieldBuilder{Name = "Watchers", Value = repo.Watchers ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Stars", Value = repo.Stars ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Open issues", Value = repo.OpenIssues ,IsInline = true});

            EmbedBuilder builder =  SimpleEmbed.FieldsEmbed(repo.Name, fields);
            builder.WithUrl(repo.URL);
            builder.WithColor(Color.Teal);
            builder.WithFooter("General Info");

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [SlashCommand("release", "Get information about the latest release.")]
        public async Task ReleaseCommand()
        {
            string response;
            List<string> headersList = new List<string>() { "application/vnd.github.v3+json" };

            try
            {
                response = await HttpRequests.SimpleGetRequest(
                "https://api.github.com/repos/mapmanagement/AivaptDotNet/releases/latest",
                headersList,
                ".NET Foundation Repository Reporter"
            );
            }
            catch(HttpRequestException)
            {
                EmbedBuilder errorBuilder =  SimpleEmbed.MinimalEmbed("Error", "Couldn't find any release!");
                errorBuilder.WithFooter("Latest Release");
                await Context.Channel.SendMessageAsync("", false, errorBuilder.Build());
                return;
            }
            
            Release rel = JsonSerializer.Deserialize<Release>(response);
            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>();
            fields.Add(new EmbedFieldBuilder{Name = "Author", Value = rel.Author, IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Created at", Value = rel.CreatedAt.ToString("HH:mm | dd.MM.yyyy") ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Pusblished at", Value = rel.PublishedAt.ToString("HH:mm | dd.MM.yyyy") ,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Text", Value = rel.Body ,IsInline = false});

            EmbedBuilder builder =  SimpleEmbed.FieldsEmbed(rel.ID.ToString(), rel.Name, fields);
            builder.WithUrl(rel.URL);
            builder.WithColor(Color.Teal);
            builder.WithFooter("Latest Release");

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [SlashCommand("issue", "Get a specific issues by its number.")]
        public async Task IssueCommand(int number)
        {
            string response;
            List<string> headersList = new List<string>() { "application/vnd.github.v3+json" };

            try
            {
                response = await HttpRequests.SimpleGetRequest(
                $"https://api.github.com/repos/mapmanagement/AivaptDotNet/issues/{number}",
                headersList,
                ".NET Foundation Repository Reporter"
            );
            }
            catch(HttpRequestException)
            {
                EmbedBuilder errorBuilder = SimpleEmbed.MinimalEmbed("Error", $"Couldn't find any issue with the number {number}");
                errorBuilder.WithFooter("Issue");
                await Context.Channel.SendMessageAsync("", false, errorBuilder.Build());
                return;
            }
            
            Issue issue = JsonSerializer.Deserialize<Issue>(response);
            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>();
            fields.Add(new EmbedFieldBuilder{Name = "State", Value = issue.State,IsInline = true});
            fields.Add(new EmbedFieldBuilder{Name = "Author", Value = issue.User.Login, IsInline = true});

            EmbedBuilder builder =  SimpleEmbed.FieldsEmbed($"{issue.Number} - {issue.Title}", issue.Body, fields);
            builder.WithUrl(issue.URL);
            builder.WithThumbnailUrl(issue.User.AvatarUrl);
            builder.WithColor(Color.Teal);
            builder.WithFooter("Issue");

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}