using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;

using AivaptDotNet.Helpers;

namespace AivaptDotNet.Modules 
{
    [Group("cmd")]
    public class SimpleCommandModule : ModuleBase<AivaptCommandContext>
    {

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
            ulong creatorId = Context.User.Id;

            Emoji greenCircle = new Emoji("🟢");
            Emoji redCircle = new Emoji("🔴");

            EmbedBuilder confirmationEmbed = SimpleEmbed.MinimalEmbed("Delete Command?");
            confirmationEmbed.WithDescription("Do you really want to delete this command?");
            RestUserMessage confirmationMsg = await Context.Channel.SendMessageAsync("", false, confirmationEmbed.Build());

            await confirmationMsg.AddReactionsAsync(new Emoji[] {greenCircle, redCircle});

            /*Context.Client.ReactionAdded += ReactionAdded_EventAsync;
            await Task.Delay(10000);
            Context.Client.ReactionAdded -= ReactionAdded_EventAsync;*/
        }

        /*public async Task ReactionAdded_EventAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            Emoji greenCircle = new Emoji("🟢");
            Emoji redCircle = new Emoji("🔴");
            
            if(cachedMessage.Id == confirmationMsg.Id && reaction.UserId == creatorId)
                    {
                        if(reaction.Emote.Name == redCircle.Name)
                        {
                            await Context.Channel.SendMessageAsync("Cancelled!");
                        }
                        else if(reaction.Emote.Name == greenCircle.Name)
                        {
                            string sql = @"delete from simple_command where name = @NAME and creator = @CREATOR"; //TODO: add specific users that can delete any commands
                            var param = new Dictionary<string, object>();
                            param.Add("@NAME", );
                            param.Add("@CREATOR", creatorId.ToString());

                            Context._dbConnector.ExecuteDML(sql, param);

                            await Context.Channel.SendMessageAsync("Command has been deleted!");
                        }
                    }
        }*/
    }
}