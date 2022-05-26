using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

using AivaptDotNet.Helpers.General;
using AivaptDotNet.Services;
using AivaptDotNet.DataClasses;

using AivaptDotNet.Helpers.Modules;
using Discord.Interactions;

namespace AivaptDotNet.Modules
{
    [Discord.Interactions.Group("cmd", "Manage simple commands")]
    public class SimpleCommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        #region Fields

        const string QuoteDeleteButtonId = "qdelete:";
        const string QuoteCancelButtonId = "qcancel:";


        #endregion

        #region Properties

        public DatabaseService DbService { get; set; }

        #endregion

        #region Commands

        [SlashCommand("create", "Create a new simplec command.")]
        public async Task CreateCmdCommand(string name, [Remainder] string text)
        {
            SimpleCommandHelper.CreateSimpleCommand(DbService, name, text, Context.User.Id);

            await Context.Channel.SendMessageAsync("Success!");
        }

        [SlashCommand("edit", "Edit a specific simple command.")]
        public async Task EditCmdCommand(string name, string title, [Remainder] string newText)
        {
            SimpleCommandHelper.EditSimpleCommand(DbService, name, newText);

            await Context.Channel.SendMessageAsync("Success!");
        }

        [SlashCommand("del", "Delete a specific simple command.")]
        public async Task DeleteCmdCommand(string name)
        {
            ulong userMsgId = Context.User.Id;
            bool commandAvailable = SimpleCommandHelper.IsCommandAvailable(DbService, name);

            if(!commandAvailable)
            {
                await RespondAsync("This command does not exist!");
                return;
            }

            IGuildUser guildUser = Context.Guild.GetUser(Context.User.Id);

            //TODO: build button-ID
            var deleteButtonId = $"{QuoteDeleteButtonId}{name},{userMsgId}";

            var buttons = new List<ButtonBuilder>()
            {
                { new ButtonBuilder("Delete", deleteButtonId, ButtonStyle.Danger) },
                { new ButtonBuilder("Cancel", QuoteCancelButtonId, ButtonStyle.Secondary) }
            };
            var buttonComponent = SimpleComponents.MultipleButtons(buttons);

            EmbedBuilder confirmationEmbed = SimpleEmbed.MinimalEmbed("Confirmation");
            confirmationEmbed.WithDescription("Do you want to delete this command?");

            await RespondAsync(components: buttonComponent.Build(), embed: confirmationEmbed.Build());

        }

        [ComponentInteraction(QuoteDeleteButtonId+"*,*")]
        public async Task HandleButtonClick(string commandName, string authorId)
        {
            //TODO: check author-ID
            SimpleCommandHelper.DeleteSimpleCommand(DbService, commandName); 
            await ReplyAsync("Command has been deleted.");
        }

        [SlashCommand("all", "Get a list of all simple commands")]
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