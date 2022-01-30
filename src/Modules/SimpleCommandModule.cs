using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using AivaptDotNet.Helpers;
using AivaptDotNet.Services;
using AivaptDotNet.DataClasses;

using MySql.Data.MySqlClient;

namespace AivaptDotNet.Modules
{
    [Group("cmd")]
    public class SimpleCommandModule : ModuleBase<CommandContext>
    {
        public DatabaseService DbService { get; set; }
        public EventService EventService { get; set; }

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

            DbService.ExecuteDML(sql, param);

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

            DbService.ExecuteDML(sql, param);

            await Context.Channel.SendMessageAsync("Success!");
        }

        [Command("del")]
        [Summary("Deletes a simple command")]
        public async Task DeleteCmdCommand(string name)
        {
            ulong userMsgId = Context.Message.Id;
            ulong? cmdAuthorId = GetCommandAuthor(name);

            if(cmdAuthorId == null)
            {
                await Context.Message.ReplyAsync("This command does not exist!");
                return;
            }

            IGuildUser guildUser = await Context.Guild.GetUserAsync(Context.User.Id);
            if (!IsUserMod(guildUser.RoleIds) && guildUser.Id != Credentials.GetAdminId() && cmdAuthorId != guildUser.Id)
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
            ButtonClickKeyword keyword = new ButtonClickKeyword(userMsgId, confirmationMsg.Id, guildUser.Id, parameters);
            EventService.AddButtonEvent(keyword, confirmationMsg.Id.ToString());
        }

        [Command("all")]
        [Summary("Shows all simple commands")]
        public async Task AllCmdsCommand()
        {
            string creator = Context.User.Id.ToString();

            string sql = @"select name, creator_id from simple_command";

            List<EmbedFieldBuilder> cmdDict = new List<EmbedFieldBuilder>();

            using (var reader = DbService.ExecuteSelect(sql, new Dictionary<string, object>()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ulong userId = reader.GetUInt64("creator_id");
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

        private bool IsUserMod(IReadOnlyCollection<ulong> roleIds)
        {
            var sqlParam = new Dictionary<string, object> { { "GUILD", Context.Guild.Id } };
            string sql = "select role_id from role where guild_id = @GUILD and mod_permissions = 1";
            using (MySqlDataReader dbRoles = DbService.ExecuteSelect(sql, sqlParam))
            {
                while (dbRoles.Read())
                {
                    foreach (var roleId in roleIds)
                    {
                        if (roleId == dbRoles.GetUInt64(0))
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

            using (MySqlDataReader reader = DbService.ExecuteSelect(sql, param))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetUInt64(0);
                    }
                }
            }
            return null;
        }

        #endregion
    }
}