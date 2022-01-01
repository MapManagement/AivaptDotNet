using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

using AivaptDotNet.Helpers;
using AivaptDotNet.AivaptClases;

using MySql.Data.MySqlClient;

namespace AivaptDotNet.Modules
{
    [Group("cmd")]
    public class SimpleCommandModule : ModuleBase<AivaptCommandContext>
    {
        private MemoryCache Cache = new MemoryCache(0, "SimpleCommandModule", 120);

        #region Modules

        [Command("create")]
        [Summary("Creates new simple command")]
        public async Task CreateCmdCommand(string name, string title, [Remainder] string text)
        {
            string creator = Context.User.Id.ToString();

            string sql = @"insert into simple_command (name, command_text, title, active, creator) values (@NAME, @TEXT, @TITLE, 1, @CREATOR)";
            var param = new Dictionary<string, object>();
            param.Add("@NAME", name);
            param.Add("@TEXT", text);
            param.Add("@TITLE", title);
            param.Add("@CREATOR", creator);

            Context._dbConnector.ExecuteDML(sql, param);

            await Context.Channel.SendMessageAsync("Success!");
        }

        [Command("edit")]
        [Summary("Edits a simple command")]
        public async Task EditCmdCommand(string name, string title, [Remainder] string text)
        {
            string sql = @"update simple_command set title = @TITLE, command_text = @TEXT where name = @NAME";
            var param = new Dictionary<string, object>();
            param.Add("@NAME", name);
            param.Add("@TEXT", text);
            param.Add("@TITLE", title);

            Context._dbConnector.ExecuteDML(sql, param);

            await Context.Channel.SendMessageAsync("Success!");
        }

        [Command("del")]
        [Summary("Deletes a simple command")]
        public async Task DeleteCmdCommand(string name)
        {
            ulong userMsgId = Context.Message.Id;
            ulong? cmdAuthorId = GetCommandAuthor(name);

            SocketGuildUser guildUser = Context.Guild.GetUser(Context.User.Id);
            if (!IsUserMod(guildUser.Roles) && guildUser.Id != Context.Client.AdminUserId && cmdAuthorId != guildUser.Id)
            {
                await Context.Message.ReplyAsync("You do not have the permissions to delete this command!");
                return;
            }

            var buttons = new List<ButtonBuilder>()
            {
                { new ButtonBuilder("Delete", $"del-{userMsgId}", ButtonStyle.Danger) },
                { new ButtonBuilder("Cancel", $"cancel-{userMsgId}", ButtonStyle.Secondary) }
            };
            var buttonComponent = SimpleComponents.MultipleButtons(buttons);

            EmbedBuilder confirmationEmbed = SimpleEmbed.MinimalEmbed("Confirmation");
            confirmationEmbed.WithDescription("Do you want to delete this command?");

            var confirmationMsg = await ReplyAsync(components: buttonComponent.Build(), embed: confirmationEmbed.Build());

            var parameters = new Dictionary<string, object>() { { "name", name } };
            ReactionKeywords keywords = new ReactionKeywords(userMsgId, confirmationMsg.Id, guildUser.Id, ButtonExecuted_EventAsync, AivaptClases.EventType.ButtonClick, parameters);
            var item = new CacheKeyValue(confirmationMsg.Id.ToString(), keywords, DateTime.Now.AddMinutes(2));
            Cache.AddKeyValue(item);


            Context.Client.ButtonExecuted += ButtonExecuted_EventAsync;
        }

        [Command("all")]
        [Summary("Shows all simple commands")]
        public async Task AllCmdsCommand()
        {
            string creator = Context.User.Id.ToString();

            string sql = @"select name, creator from simple_command";

            List<EmbedFieldBuilder> cmdDict = new List<EmbedFieldBuilder>();

            using (var reader = Context._dbConnector.ExecuteSelect(sql, new Dictionary<string, object>()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var userId = ulong.Parse(reader.GetString("creator"));
                        IUser user = await Context.Client.GetUserAsync(userId);
                        cmdDict.Add(new EmbedFieldBuilder { Name = reader.GetString("name"), Value = user.Username });
                    }
                }
            }
            EmbedBuilder builder = SimpleEmbed.FieldsEmbed("All User Commands", cmdDict);
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        #endregion

        #region Methods

        private bool IsUserMod(IReadOnlyCollection<SocketRole> roles)
        {
            var sqlParam = new Dictionary<string, object> { { "GUILD", Context.Guild.Id } };
            string sql = "select role_id from roles where guild_id = @GUILD and mod_permissions = 1";
            using (MySqlDataReader dbRoles = Context._dbConnector.ExecuteSelect(sql, sqlParam))
            {
                while (dbRoles.Read())
                {
                    foreach (var role in roles)
                    {
                        if (role.Id == dbRoles.GetUInt64(0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private ulong? GetCommandAuthor(string commandName)
        {
            string sql = "select creator from simple_command where name = @COMMAND";
            var param = new Dictionary<string, object>();
            param.Add("@COMMAND", commandName);

            using (MySqlDataReader reader = Context._dbConnector.ExecuteSelect(sql, param))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var idString = reader.GetString(0);
                        return ulong.Parse(idString);
                    }
                }
            }

            return null;
        }

        #endregion

        #region Events

        private async Task ButtonExecuted_EventAsync(SocketMessageComponent arg)
        {
            ulong userId = arg.User.Id;
            ulong messageId = arg.Message.Id;
            var buttonId = arg.Data.CustomId;

            var keyValue = Cache.GetKeyValue(messageId.ToString()) as CacheKeyValue;
            var keywords = keyValue?.Value as ReactionKeywords;
            if (keywords == null) return;

            string commandName = keywords.Parameters["name"] as string;

            if (messageId == keywords.BotMessageId && (userId == keywords.AuthorId))
            {
                if (buttonId == $"del-{keywords.UserMessageId}")
                {
                    string sql = @"delete from simple_command where name = @NAME and creator = @CREATOR";
                    var param = new Dictionary<string, object>();
                    param.Add("@NAME", commandName);
                    param.Add("@CREATOR", keywords.AuthorId.ToString());

                    Context._dbConnector.ExecuteDML(sql, param);

                    await arg.UpdateAsync(x =>
                    {
                        x.Embed = SimpleEmbed.MinimalEmbed("Confirmation")
                            .WithDescription("Command has been deleted!")
                            .WithFooter(arg.User.Username, arg.User.GetAvatarUrl())
                            .Build();

                        var buttons = new List<ButtonBuilder>()
                        {
                            { new ButtonBuilder("Delete", $"del-dis", ButtonStyle.Danger, isDisabled: true) },
                            { new ButtonBuilder("Cancel", $"cancel-dis", ButtonStyle.Secondary, isDisabled: true) }
                        };
                        var buttonComponent = SimpleComponents.MultipleButtons(buttons);
                        x.Components = buttonComponent.Build();
                    });

                    await arg.RespondAsync("Command has beed deleted!");

                    Context.Client.ButtonExecuted -= ButtonExecuted_EventAsync;
                }
                else if (buttonId == $"cancel-{keywords.UserMessageId}")
                {
                    await arg.UpdateAsync(x =>
                    {
                        x.Embed = SimpleEmbed.MinimalEmbed("Confirmation")
                            .WithDescription("Cancelled!")
                            .WithFooter(arg.User.Username, arg.User.GetAvatarUrl())
                            .Build();

                        var buttons = new List<ButtonBuilder>()
                        {
                            { new ButtonBuilder("Delete", $"del-dis", ButtonStyle.Danger, isDisabled: true) },
                            { new ButtonBuilder("Cancel", $"cancel-dis", ButtonStyle.Secondary, isDisabled: true) }
                        };
                        var buttonComponent = SimpleComponents.MultipleButtons(buttons);
                        x.Components = buttonComponent.Build();
                    });

                    Context.Client.ButtonExecuted -= ButtonExecuted_EventAsync;
                }
            }
        }

        #endregion
    }
}