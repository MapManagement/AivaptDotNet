using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AivaptDotNet.Helpers;
using AivaptDotNet.Helpers.Discord;
using AivaptDotNet.Services.Database;
using AivaptDotNet.Services.Database.Models;

namespace AivaptDotNet.Modules
{
    [Group("quote", "Manage quotes of different users on a server")]
    public class QuoteModule : InteractionModuleBase<SocketInteractionContext>
    {
        public BotDbContext DbContext { get; set; }

        [SlashCommand("new", "Create a new quote.")]
        public async Task NewQuoteCommand(SocketUser user, string quoteText)
        {
            QuoteHelper.InsertQuote(DbContext, user, quoteText);

            await RespondAsync("New quote has been created!");
        }

        [SlashCommand("show", "Get a specific quote by ID.")]
        public async Task ShowQuoteCommand(int quoteId)
        {
            Quote quote = QuoteHelper.GetQuote(DbContext, quoteId);

            IUser user = await Context.Client.GetUserAsync(quote.UserId);

            if (user == null)
                return;

            EmbedBuilder builder = SimpleEmbed.MinimalEmbed($"#{quote.Id}", quote.Text);
            builder.WithTimestamp(quote.CreatedAt);
            builder.WithAuthor(user.Username, user.GetAvatarUrl());

            await RespondAsync(embed: builder.Build());
        }

        [SlashCommand("random", "Get a random quote.")]
        public async Task ShowRandomQuoteCommand()
        {
            Quote quote = QuoteHelper.GetRandomQuote(DbContext);

            IUser user = await Context.Client.GetUserAsync(quote.UserId);

            if (user == null)
                return;

            EmbedBuilder builder = SimpleEmbed.MinimalEmbed($"#{quote.Id}", quote.Text);
            builder.WithTimestamp(quote.CreatedAt);
            builder.WithAuthor(user.Username, user.GetAvatarUrl());

            await RespondAsync(embed: builder.Build());
        }

        [SlashCommand("amount", "Get the amount of all quote that have been created so far.")]
        public async Task QuoteAmountCommand()
        {
            int amount = QuoteHelper.GetAmountOfQuotes(DbContext);

            await RespondAsync($"There are currently {amount} quotes.");
        }
    }
}
