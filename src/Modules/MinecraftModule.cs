using AivaptDotNet.Helpers.Modules;
using AivaptDotNet.Services.Database;
using Discord;
using Discord.Interactions;
using System.Threading.Tasks;

namespace AivaptDotNet.Modules
{
    [Group("mc", "Minecraft stuff")]
    public class MinecraftModule : InteractionModuleBase<SocketInteractionContext>
    {
        public BotDbContext DbContext { get; set; }

        #region Location Types

        [Group("type", "Create Minecraft location types")]
        public class TypeModule : InteractionModuleBase<SocketInteractionContext>
        {
            public BotDbContext DbContext { get; set; }

            #region Slash Commands

            [SlashCommand("new", "Add a new location type")]
            public async Task NewLocationType(string name)
            {
                var output = MinecraftHelper.InsertLocationType(DbContext, name);
                await RespondAsync(output);
            }

            [SlashCommand("delete", "Delete an existing location type")]
            public async Task DeleteLocationType(string name)
            {
                var output = MinecraftHelper.DeleteLocationType(DbContext, name);
                await RespondAsync(output);
            }

            [SlashCommand("list", "Lists all existing location types")]
            public async Task ListLocationTypes()
            {
                var embed = MinecraftHelper.ListLocationTypes(DbContext);
                await RespondAsync(null, embed: embed);
            }

            #endregion
        }

        #endregion

        #region Coordinates

        [Group("coordinates", "Set new Minecraft coordinates for special loot")]
        public class CoordinatesModule : InteractionModuleBase<SocketInteractionContext>
        {
            public BotDbContext DbContext { get; set; }

            #region Slash Commands

            [SlashCommand("new", "Add new coordinates")]
            public async Task NewCoordinates(ulong x, ulong y, ulong z = 0)
            {
                var locationTypes = MinecraftHelper.GetAllLocationTypes(DbContext);

                // TODO: add custom ID
                var selectMenuBuilder = new SelectMenuBuilder()
                    .WithPlaceholder("Select a Minecraft location type")
                    .WithCustomId("mc-select-1")
                    .WithMinValues(1);

                foreach (var locationType in locationTypes)
                {
                    selectMenuBuilder.AddOption(locationType.LocationName,
                                                $"mc-location_type-{locationType.LocationId}");
                }

                var component = new ComponentBuilder()
                    .WithSelectMenu(selectMenuBuilder)
                    .Build();

                await RespondAsync("What location type should be stored?", components: component);

                // TODO: add event handler
            }

            #endregion

            #region Component Interaction

            [ComponentInteraction("mc-location-type-*")]
            public async Task HandleCoordinatesSelectMenu(string id, string[] locationTypes)
            {
                var s = "Selected: ";

                foreach (var lt in locationTypes)
                {
                    s += lt;
                }

                await RespondAsync(s);
            }

            #endregion
        }

        #endregion
    }
}
