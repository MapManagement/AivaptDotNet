using AivaptDotNet.Helpers.DiscordClasses;
using AivaptDotNet.Helpers.Modules;
using AivaptDotNet.Services.Database;
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
                var customId = $"mc-coordinates-new,{x},{y},{z}";

                var selectMenuBuilder = new SelectMenuBuilder()
                {
                    CustomId = customId,
                    Placeholder = "Select a Minecraft location type...",
                    MinValues = 1,
                    MaxValues = 5
                };

                foreach (var locationType in locationTypes)
                {
                    var optionId = $"mc_location_type,{locationType.LocationId}";
                    selectMenuBuilder.AddOption(locationType.LocationName, optionId);
                }

                var component = new ComponentBuilder()
                    .WithSelectMenu(selectMenuBuilder)
                    .Build();

                await RespondAsync("What location type should be stored?", components: component);
            }

            [SlashCommand("type", "Get all coordinates that are linked to a certain location type")]
            public async Task GetCoordinatesType()
            {
                // TODO: add SimpleSelectMenu to helper classes
                var locationTypes = MinecraftHelper.GetAllLocationTypes(DbContext);

                var selectMenuBuilder = new SelectMenuBuilder()
                {
                    CustomId = "mc_coordinates_type",
                    Placeholder = "Select a Minecraft location type...",
                };

                foreach (var locationType in locationTypes)
                {
                    var optionId = $"mc_location_type,{locationType.LocationId}";
                    selectMenuBuilder.AddOption(locationType.LocationName, optionId);
                }

                var component = new ComponentBuilder()
                    .WithSelectMenu(selectMenuBuilder)
                    .Build();

                await RespondAsync("What location type are you looking for?", components: component, ephemeral: true);
            }

            #endregion

            #region Component Interaction

            [ComponentInteraction("mc-coordinates-new,*,*,*", true)]
            public async Task HandleNewCoordinatesSelectMenu(string x, string y, string z,
                                                          string[] locationTypes)
            {
                // TODO: disable select menu
                var xLong = Convert.ToUInt64(x);
                var yLong = Convert.ToUInt64(y);
                var zLong = Convert.ToUInt64(z);

                string response = MinecraftHelper
                    .InsertCoordinates(DbContext, xLong, yLong, zLong, locationTypes,
                                       Context.User.Id);

                await ReplyAsync(response);
            }

            [ComponentInteraction("mc_coordinates_type", true)]
            public async Task HandleTypeCoordinatesSelectMenu(string[] locationTypes)
            {
                var message = this.Context.Interaction as SocketMessageComponent;

                if (locationTypes.Length == 0)
                    await RespondAsync("Something went wrong...");

                var customId = locationTypes[0];
                var embed = MinecraftHelper.ListCoordinatesByTypeId(DbContext, customId);

                await message.UpdateAsync(msg =>
                {
                    msg.Content = "Location type has already been chosen.";
                    msg.Components = null; //TODO: cannot update components, needs to be set to null
                });

                await FollowupAsync(embed: embed);
            }

            #endregion
        }

        #endregion
    }
}
