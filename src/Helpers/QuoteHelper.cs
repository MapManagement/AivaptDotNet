using AivaptDotNet.Services.Database;
using AivaptDotNet.Services.Database.Models;
using Discord.WebSocket;
using System;
using System.Linq;

namespace AivaptDotNet.Helpers
{
    public static class QuoteHelper
    {
        #region Methods

        public static void InsertQuote(BotDbContext dbContext, SocketUser user, string quoteText)
        {
            var quote = new Quote
            {
                UserId = user.Id,
                Text = quoteText
            };

            dbContext.Quotes.Add(quote);
            dbContext.SaveChanges();
        }

        public static Quote GetQuote(BotDbContext dbContext, int quoteId)
        {
            var quote = dbContext.Quotes.Find(quoteId);

            return quote;
        }

        public static Quote GetRandomQuote(BotDbContext dbContext)
        {
            var quote = dbContext.Quotes
                .OrderBy(q => Guid.NewGuid())
                .First();

            return quote;
        }

        public static int GetAmountOfQuotes(BotDbContext dbContext)
        {
            var number = dbContext.Quotes.Count();

            return number;
        }

        #endregion
    }
}
