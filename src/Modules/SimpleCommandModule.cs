using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using AivaptDotNet.Helpers.General;
using AivaptDotNet.Services;
using AivaptDotNet.DataClasses;

using MySql.Data.MySqlClient;
using AivaptDotNet.Helpers.Modules;

namespace AivaptDotNet.Modules
{
    [Group("cmd")]
    public class SimpleCommandModule : ModuleBase<CommandContext>
    {
        #region Properties

        public DatabaseService DbService { get; set; }
        public EventService EventService { get; set; }

        #endregion

        #region Commands

        [Command("create")]
        [Summary("Creates new simple command")]
        public async Task CreateCmdCommand(string name, [Remainder] string text)
        {
            SimpleCommandHelper.CreateSimpleCommand(DbService, name, text, Context.User.Id);

            await Context.Channel.SendMessageAsync("Success!");
        }

        [Command("edit")]
        [Summary("Edits a simple command")]
        public async Task EditCmdCommand(string name, string title, [Remainder] string newText)
        {
            SimpleCommandHelper.EditSimpleCommand(DbService, name, newText);

            await Context.Channel.SendMessageAsync("Success!");
        }

        [Command("del")]
        [Summary("Deletes a simple command")]
        public async Task DeleteCmdCommand(string name)
        {
            ulong userMsgId = Context.Message.Id;
            bool commandAvailable = SimpleCommandHelper.IsCommandAvailable(DbService, name);

            if(!commandAvailable)
            {
                await Context.Message.ReplyAsync("This command does not exist!");
                return;
            }

            IGuildUser guildUser = await Context.Guild.GetUserAsync(Context.User.Id);

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
            var commands = SimpleCommandHelper.GetAllSimpleCommands(DbService);
            List<EmbedFieldBuilder> embedFields = new List<EmbedFieldBuilder>();

            foreach (var command in commands.Keys)
            {
                IUser user = await Context.Client.GetUserAsync(commands[command]);
                embedFields.Add(new EmbedFieldBuilder { Name = command, Value = user.Username });
            }

            EmbedBuilder builder = SimpleEmbed.FieldsEmbed("All User Commands", embedFields);
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        #endregion
    }
}