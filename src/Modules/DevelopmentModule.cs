using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.Interactions;
using AivaptDotNet.DataClasses;
using AivaptDotNet.Helpers;
using AivaptDotNet.Helpers.Discord;

namespace AivaptDotNet.Modules
{
    [Group("dev", "Check the developtment status of the AivaptDotNet GitHub repository")]
    public class DevelopmentModule : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("latest", "Get the latest changes.")]
        public async Task LatestCommand()
        {
            Commit latestCommit = DevelopmentHelper.GetLatestCommit();

            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder{Name = "Author", Value = latestCommit.Author.Login, IsInline = false},
                new EmbedFieldBuilder{Name = "Message", Value = latestCommit.Details.Message, IsInline = false}
            };

            EmbedBuilder builder = SimpleEmbed.FieldsEmbed($"{latestCommit.SHA.Substring(0, 8)}", fields)
                .WithUrl(latestCommit.URL)
                .WithThumbnailUrl(latestCommit.Author.AvatarUrl)
                .WithColor(Color.Teal)
                .WithFooter("Latest Commit");

            await RespondAsync(embed: builder.Build());
        }

        [SlashCommand("info", "Get general information about the repository.")]
        public async Task InfoCommand()
        {
            Repository repo = DevelopmentHelper.GetGeneralInformation();

            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder{Name = "ID", Value = repo.ID ,IsInline = true},
                new EmbedFieldBuilder{Name = "Created at", Value = repo.CreatedAt.ToString("HH:mm | dd.MM.yyyy") ,IsInline = true},
                new EmbedFieldBuilder{Name = "Last updated", Value = repo.UpdatedAt.ToString("HH:mm | dd.MM.yyyy") ,IsInline = true},
                new EmbedFieldBuilder{Name = "Languages", Value = repo.Language ,IsInline = true},
                new EmbedFieldBuilder{Name = "Watchers", Value = repo.Watchers ,IsInline = true},
                new EmbedFieldBuilder{Name = "Stars", Value = repo.Stars ,IsInline = true},
                new EmbedFieldBuilder{Name = "Open issues", Value = repo.OpenIssues ,IsInline = true}
            };

            EmbedBuilder builder = SimpleEmbed.FieldsEmbed(repo.Name, fields)
                .WithUrl(repo.URL)
                .WithColor(Color.Teal)
                .WithFooter("General Info");

            await RespondAsync(embed: builder.Build());
        }

        [SlashCommand("release", "Get information about the latest release.")]
        public async Task ReleaseCommand()
        {
            Release latestRelease = await DevelopmentHelper.GetLatestReleaseAsync();

            if (latestRelease == null)
            {
                EmbedBuilder errorBuilder = SimpleEmbed.MinimalEmbed("Error", "Couldn't find any release!");
                errorBuilder.WithFooter("Latest Release");
                await RespondAsync(embed: errorBuilder.Build());
                return;
            }

            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder{Name = "Author", Value = latestRelease.Author, IsInline = true},
                new EmbedFieldBuilder{Name = "Created at", Value = latestRelease.CreatedAt.ToString("HH:mm | dd.MM.yyyy") ,IsInline = true},
                new EmbedFieldBuilder{Name = "Pusblished at", Value = latestRelease.PublishedAt.ToString("HH:mm | dd.MM.yyyy") ,IsInline = true},
                new EmbedFieldBuilder{Name = "Text", Value = latestRelease.Body ,IsInline = false}
            };

            EmbedBuilder builder = SimpleEmbed.FieldsEmbed(latestRelease.ID.ToString(), latestRelease.Name, fields)
                .WithUrl(latestRelease.URL)
                .WithColor(Color.Teal)
                .WithFooter("Latest Release");

            await RespondAsync(embed: builder.Build());
        }

        [SlashCommand("issue", "Get a specific issue by its number.")]
        public async Task IssueCommand(int number)
        {
            Issue issue = await DevelopmentHelper.GetIssueAsync(number);

            if (issue == null)
            {
                EmbedBuilder errorBuilder = SimpleEmbed.MinimalEmbed("Error", $"Couldn't find any issue with the number {number}");
                errorBuilder.WithFooter("Issue");
                await RespondAsync(embed: errorBuilder.Build());
                return;
            }

            List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder{Name = "State", Value = issue.State,IsInline = true},
                new EmbedFieldBuilder{Name = "Author", Value = issue.User.Login, IsInline = true}
            };

            EmbedBuilder builder = SimpleEmbed.FieldsEmbed($"{issue.Number} - {issue.Title}", issue.Body, fields)
                .WithUrl(issue.URL)
                .WithThumbnailUrl(issue.User.AvatarUrl)
                .WithColor(Color.Teal)
                .WithFooter("Issue");

            await RespondAsync(embed: builder.Build());
        }
    }
}
