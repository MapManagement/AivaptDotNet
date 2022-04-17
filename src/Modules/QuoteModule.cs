using System;
using System.Threading.Tasks;
using System.IO;

using Discord.Commands;
using Discord;

using AivaptDotNet.Helpers.General;
using AivaptDotNet.Helpers.Modules;
using AivaptDotNet.DataClasses;
using Discord.WebSocket;
using AivaptDotNet.Services;
using System.Collections.Generic;

namespace AivaptDotNet.Modules 
{
    [Group("quote")]
    public class QuoteModule : ModuleBase<CommandContext>
    {
        public DatabaseService DbService { get; set; }

        [Command("new")]
        [Summary("Create a new quote")]
        public async Task NewQuoteCommand(SocketUser user, string quoteText)
        {   
            QuoteHelper.InsertQuote(DbService, user, quoteText);

            await ReplyAsync("New quote has been created!");
        }

        [Command("show")]
        [Summary("Returns a quote")]
        public async Task ShowQuoteCommand(int quoteId)
        {   
            Quote quote = QuoteHelper.GetQuote(DbService, quoteId);

            IUser user = await Context.Client.GetUserAsync(quote.UserId);

            if ( user == null)
                return;

            EmbedBuilder builder = SimpleEmbed.MinimalEmbed($"#{quote.Id}", quote.Text);
            builder.WithTimestamp(quote.CreatedAt);
            builder.WithAuthor(user.Username, user.GetAvatarUrl());

            await ReplyAsync("", false, builder.Build());

        }

        [Command("random")]
        [Summary("Returns a random quote")]
        public async Task ShowRandomQuoteCommand()
        {   
            Quote quote = QuoteHelper.GetRandomQuote(DbService);

            IUser user = await Context.Client.GetUserAsync(quote.UserId);

            if (user == null)
                return;

            EmbedBuilder builder = SimpleEmbed.MinimalEmbed($"#{quote.Id}", quote.Text);
            builder.WithTimestamp(quote.CreatedAt);
            builder.WithAuthor(user.Username, user.GetAvatarUrl());

            await ReplyAsync("", false, builder.Build());

        }

        [Command("amount")]
        [Summary("Returns the amount of quotes that exist")]
        public async Task QuoteAmountCommand()
        {   
            int amount = QuoteHelper.GetAmountOfQuotes(DbService);

            await ReplyAsync($"There are currently {amount} quotes.");

        }
    }
}