using System;
using System.Threading.Tasks;
using System.IO;

using Discord.Commands;
using Discord;

using AivaptDotNet.Helpers.General;
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
        public async Task NewQuoteCommand(SocketUser user, string quote)
        {   
            string sql = "insert into quote (user_id, text, created_at) values (@USER_ID, @TEXT, sysdate())";
            var param = new Dictionary<string, object>
            {
                { "@USER_ID", user.Id },
                { "@TEXT", quote },
            };

            DbService.ExecuteDML(sql, param);

            await ReplyAsync("New quote has been created!");
        }

        [Command("show")]
        [Summary("Returns a quote")]
        public async Task ShowQuoteCommand(int quoteId)
        {   
            string sql = "select * from quote where id = @QUOTE_ID";
            var param = new Dictionary<string, object>
            {
                { "@QUOTE_ID", quoteId }
            };

            using var result = DbService.ExecuteSelect(sql, param);

            if(!result.HasRows)
                await ReplyAsync($"There's no quote #{quoteId}");

            result.Read();

            UInt64 id = result.GetUInt64("id");
            UInt64 userId = result.GetUInt64("user_id");
            string text = result.GetString("text");
            DateTime createdAt = result.GetDateTime("created_at");

            IUser user = await Context.Client.GetUserAsync(userId);

            EmbedBuilder builder = SimpleEmbed.MinimalEmbed($"#{id}", text);
            builder.WithTimestamp(createdAt);
            builder.WithAuthor(user.Username, user.GetAvatarUrl());

            await ReplyAsync("", false, builder.Build());

        }
    }
}