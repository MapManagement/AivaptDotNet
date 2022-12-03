using System.Collections.Generic;
using AivaptDotNet.DataClasses;
using AivaptDotNet.Services;
using Discord.WebSocket;

namespace AivaptDotNet.Helpers.Modules
{
    public static class QuoteHelper
    {
        #region Methods

        public static void InsertQuote(DatabaseService dbService, SocketUser user, string quoteText)
        {
            string sql = "insert into quote (user_id, text, created_at) values (@USER_ID, @TEXT, sysdate())";
            var param = new Dictionary<string, object>
            {
                { "@USER_ID", user.Id },
                { "@TEXT", quoteText },
            };

            dbService.ExecuteDML(sql, param);
        }

        public static Quote GetQuote(DatabaseService dbService, int quoteId)
        {
            string sql = "select * from quote where id = @QUOTE_ID";
            var param = new Dictionary<string, object>
            {
                { "@QUOTE_ID", quoteId }
            };

            using (var result = dbService.ExecuteSelect(sql, param))
			{
            	if (!result.HasRows)
                	return null;

            	result.Read();

            	var quote = new Quote
            	(
                	id: result.GetUInt64("id"),
                	userId: result.GetUInt64("user_id"),
                	text: result.GetString("text"),
                	createdAt: result.GetDateTime("created_at")
            	);

            	return quote;
			}
        }

        public static Quote GetRandomQuote(DatabaseService dbService)
        {
            string sql = "select * from quote order by rand() limit 1";

            using var result = dbService.ExecuteSelect(sql);

            if (!result.HasRows)
                return null;

            result.Read();

            var quote = new Quote
            (
                id: result.GetUInt64("id"),
                userId: result.GetUInt64("user_id"),
                text: result.GetString("text"),
                createdAt: result.GetDateTime("created_at")
            );

            return quote;
        }

        public static int GetAmountOfQuotes(DatabaseService dbService)
        {
            string sql = "select sum(1) amount from quote";

            using (var result = dbService.ExecuteSelect(sql))
			{
            	if (!result.HasRows)
                	return 0;

            	result.Read();

            	return result.GetInt32("amount");
			}
        }

        #endregion
    }
}
