using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.Interactions;
using AivaptDotNet.Services.Database;
using AivaptDotNet.Helpers;
using AivaptDotNet.Helpers.Discord;

namespace AivaptDotNet.Modules
{
    [Group("cmd", "Manage simple commands")]
    public class SimpleCommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        #region Fields

        const string QuoteDeleteButtonId = "qdelete:";
        const string QuoteCancelButtonId = "qcancel:";

        #endregion

        #region Properties

        public BotDbContext DbContext { get; set; }

        #endregion

        #region Commands

        [SlashCommand("create", "Create a new simple command.")]
        public async Task CreateCmdCommand(string name, [Discord.Commands.Remainder] string text)
        {
            SimpleCommandHelper.CreateSimpleCommand(DbContext, name, text, Context.User.Id);

            await RespondAsync("Success!");
        }

        [SlashCommand("edit", "Edit a specific simple command.")]
        public async Task EditCmdCommand(string name, string title, [Discord.Commands.Remainder] string newText)
        {
            SimpleCommandHelper.EditSimpleCommand(DbContext, name, newText);

            await RespondAsync("Success!");
        }

        [SlashCommand("del", "Delete a specific simple command.")]
        public async Task DeleteCmdCommand(string name)
        {
            ulong userMsgId = Context.User.Id;
            bool commandAvailable = SimpleCommandHelper.IsCommandAvailable(DbContext, name);

            if (!commandAvailable)
            {
                await RespondAsync("This command does not exist!");
                return;
            }

            IGuildUser guildUser = Context.Guild.GetUser(Context.User.Id);

            //TODO: build button-ID
            var deleteButtonId = $"{QuoteDeleteButtonId}{name},{userMsgId}";

            var buttons = new List<ButtonBuilder>()
            {
                { new ButtonBuilder(label: "Delete", customId: "test", style: ButtonStyle.Danger) },
                { new ButtonBuilder("Cancel", QuoteCancelButtonId, ButtonStyle.Secondary) }
            };
            var buttonComponent = SimpleComponents.MultipleButtons(buttons);

            EmbedBuilder confirmationEmbed = SimpleEmbed.MinimalEmbed("Confirmation");
            confirmationEmbed.WithDescription("Do you want to delete this command?");

            await RespondAsync(components: buttonComponent.Build(), embed: confirmationEmbed.Build());
        }

        //TODO: not working yet
        [ComponentInteraction("test")]
        public async Task HandleButtonClick()
        {
            //TODO: check author-ID
            //SimpleCommandHelper.DeleteSimpleCommand(DbService, commandName); 
            await RespondAsync("Command has been deleted.");
        }

        [SlashCommand("all", "Get a list of all simple commands")]
        public async Task AllCmdsCommand()
        {
            var commands = SimpleCommandHelper.GetAllSimpleCommands(DbContext);
            List<EmbedFieldBuilder> embedFields = new List<EmbedFieldBuilder>();

            foreach (var command in commands)
            {
                IUser user = await Context.Client.GetUserAsync(command.CreatorId);
                embedFields.Add(new EmbedFieldBuilder { Name = command.Name, Value = user.Username });
            }

            EmbedBuilder builder = SimpleEmbed.FieldsEmbed("All User Commands", embedFields);
            await RespondAsync(embed: builder.Build());
        }

        #endregion

        #region Interaction


        #endregion
    }
}
