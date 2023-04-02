using AivaptDotNet.Helpers.Minecraft;
using AivaptDotNet.Services.Database;
using AivaptDotNet.Helpers.Discord;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
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
                var output = LocationTypeHelper.InsertLocationType(DbContext, name);
                await RespondAsync(output);
            }

            [SlashCommand("delete", "Delete an existing location type")]
            public async Task DeleteLocationType()
            {
                var selectMenuBuilder = LocationTypeHelper.GetLocationTypeSelectMenu(DbContext)
                    .WithCustomId("mc_location_type_delete")
                    .WithMaxValues(1);

                var component = new ComponentBuilder()
                    .WithSelectMenu(selectMenuBuilder)
                    .Build();

                await RespondAsync("What location type needs to be deleted?",
                                   components: component,
                                   ephemeral: true);
            }

            [SlashCommand("list", "Lists all existing location types")]
            public async Task ListLocationTypes()
            {
                var embed = LocationTypeHelper.ListLocationTypes(DbContext);
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
            public async Task NewCoordinates(long x, long y, long z = 0)
            {
                var selectMenuBuilder = LocationTypeHelper.GetLocationTypeSelectMenu(DbContext)
                    .WithCustomId($"mc_coordinates_new,{x},{y},{z}")
                    .WithMaxValues(5);

                var component = new ComponentBuilder()
                    .WithSelectMenu(selectMenuBuilder)
                    .Build();

                await RespondAsync("What location type should be stored?", components: component);
            }

            [SlashCommand("delete", "Delete existing coordinates")]
            public async Task DeleteCoordinates(int coordinatesId)
            {
                var response = CoordinatesHelper.DeleteCoordinates(DbContext, coordinatesId);
                await RespondAsync(response);
            }

            [SlashCommand("type", "Get all coordinates that are linked to a certain location type")]
            public async Task GetCoordinatesType()
            {
                var selectMenuBuilder = LocationTypeHelper.GetLocationTypeSelectMenu(DbContext)
                    .WithCustomId("mc_coordinates_type")
                    .WithMaxValues(1);

                var component = new ComponentBuilder()
                    .WithSelectMenu(selectMenuBuilder)
                    .Build();

                await RespondAsync("What location type are you looking for?",
                                   components: component,
                                   ephemeral: true);
            }

            #endregion

            #region Component Interaction

            [ComponentInteraction("mc_coordinates_new,*,*,*", true)]
            public async Task HandleNewCoordinatesSelectMenu(string x, string y, string z,
                                                             string[] locationTypes)
            {
                
                var message = this.Context.Interaction as SocketMessageComponent;

                long xLong, yLong, zLong;
                bool result = true;
                result &= long.TryParse(x, out xLong);
                result &= long.TryParse(y, out yLong);
                result &= long.TryParse(z, out zLong);

                if (!result)
                {
                    await RespondAsync("The entered coordinates are not valid.");
                    SimpleComponents.DisableSelectMenuMessage(message, "An error occured.");
                    return;
                }

                string newContent = "The location types have already been chosen.";
                SimpleComponents.DisableSelectMenuMessage(message, newContent);

                string response = CoordinatesHelper
                    .InsertCoordinates(DbContext, xLong, yLong, zLong, locationTypes,
                                       Context.User.Id);

                await FollowupAsync(response);
            }

            [ComponentInteraction("mc_coordinates_type", true)]
            public async Task HandleTypeCoordinatesSelectMenu(string[] locationTypes)
            {
                var message = this.Context.Interaction as SocketMessageComponent;
 
                if (locationTypes.Length == 0)
                {
                    await RespondAsync("Something went wrong...");
                    SimpleComponents.DisableSelectMenuMessage(message, "An error occured.");
                    return;
                }

                string newContent = "The location type has already been chosen.";
                SimpleComponents.DisableSelectMenuMessage(message, newContent);

                var customId = locationTypes[0];
                var embed = CoordinatesHelper.ListCoordinatesByLocationTypeId(DbContext, customId);

                await FollowupAsync(embed: embed);
            }

            [ComponentInteraction("mc_location_type_delete", true)]
            public async Task HandDeleteLocationTypeSelectMenu(string[] locationTypes)
            {
                var message = this.Context.Interaction as SocketMessageComponent;

                if (locationTypes.Length == 0)
                {
                    await RespondAsync("Something went wrong...");
                    SimpleComponents.DisableSelectMenuMessage(message, "An error occured.");
                    return;
                }
                    
                string newContent = "The location type has already been chosen.";
                SimpleComponents.DisableSelectMenuMessage(message, newContent);

                var customId = locationTypes[0];
                var output = LocationTypeHelper.DeleteLocationType(DbContext, customId);

                await FollowupAsync(output);
            }

            #endregion
        }

        #endregion
    }
}
