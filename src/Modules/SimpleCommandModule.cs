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

        private List<ReactionKeywords> ReactionEvents = new List<ReactionKeywords>(); //TODO: background process needed

        #region Modules

        [Command("create")]
        [Summary("Creates new simple command")]
        public async Task CreateCmdCommand(string name, string title, [Remainder]string text)
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
        public async Task EditCmdCommand(string name, string title, [Remainder]string text)
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
            SocketGuildUser guildUser =  Context.Guild.GetUser(Context.User.Id);
            if(!IsUserIsMod(guildUser.Roles) && guildUser.Id != Context.Client.AdminUserId)
            {
                await Context.Channel.SendMessageAsync("You do not have the permissions to delete a command!");
                return;
            }

            ulong creatorId = Context.User.Id;

            EmbedBuilder confirmationEmbed = SimpleEmbed.MinimalEmbed("Delete Command?");
            confirmationEmbed.WithDescription("Do you really want to delete this command?");
            RestUserMessage confirmationMsg = await Context.Channel.SendMessageAsync("", false, confirmationEmbed.Build());

            await confirmationMsg.AddReactionsAsync(new Emoji[] {ResourceManager.GreenCircleEmoji, ResourceManager.RedCircleEmoji});

            var parameters = new Dictionary<string, object>() { {"name", name} };
            ReactionKeywords keywords = new ReactionKeywords(confirmationMsg.Id, creatorId, parameters);
            ReactionEvents.Add(keywords);

            Context.Client.ReactionAdded += ReactionAdded_EventAsync;
        }

        

        [Command("all")]
        [Summary("Shows all simple commands")]
        public async Task AllCmdsCommand()
        {
            string creator = Context.User.Id.ToString();

            string sql = @"select name, creator from simple_command";

            List<EmbedFieldBuilder> cmdDict = new List<EmbedFieldBuilder>();

            using(var reader = Context._dbConnector.ExecuteSelect(sql, new Dictionary<string, object>()))
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        SocketUser user = Context.Client.GetUser(ulong.Parse(reader.GetString("creator")));
                        cmdDict.Add(new EmbedFieldBuilder{Name = reader.GetString("name"), Value = user.Username});
                    }
                }
            }
            EmbedBuilder builder = SimpleEmbed.FieldsEmbed("All User Commands", cmdDict);
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        #endregion

        #region Methods

        private bool IsUserIsMod(IReadOnlyCollection<SocketRole> roles)
        {
            var sqlParam = new Dictionary<string, object> { {"GUILD", Context.Guild.Id} };
            string sql = "select role_id from roles where guild_id = :GUILD and mod_permissions = 1";
            using (MySqlDataReader dbRoles =  Context._dbConnector.ExecuteSelect(sql, sqlParam))
            {
                while(dbRoles.Read())
                {
                    foreach(var role in roles)
                    {
                        if(role.Id == dbRoles.GetUInt64(0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
            
        }

        #endregion

        #region Events

        public async Task ReactionAdded_EventAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            ReactionKeywords keywords = ReactionEvents.Find(e => e.OriginMessageId == cachedMessage.Id);
            if(keywords == null) return;

            string commandName = keywords.Parameters["name"] as string;

            if(cachedMessage.Id == keywords.OriginMessageId  && (reaction.UserId == keywords.AuthorId || reaction.UserId == Context.Client.AdminUserId))
                {
                    if(reaction.Emote.Name == ResourceManager.GreenCircleEmoji.Name)
                    {
                        string sql = @"delete from simple_command where name = @NAME and creator = @CREATOR"; //TODO: add specific users that can delete any commands
                        var param = new Dictionary<string, object>();
                        param.Add("@NAME", commandName);
                        param.Add("@CREATOR", keywords.AuthorId.ToString());

                        Context._dbConnector.ExecuteDML(sql, param);

                        //await confirmationMsg.AddReactionAsync(ResourceManager.CheckEmoji);
                        Context.Client.ReactionAdded -= ReactionAdded_EventAsync;
                    }
                    else if(reaction.Emote.Name == ResourceManager.RedCircleEmoji.Name)
                    {
                       //await confirmationMsg.AddReactionAsync(ResourceManager.CheckEmoji);
                       Context.Client.ReactionAdded -= ReactionAdded_EventAsync;
                    }
                }
        }

        #endregion
    }
}