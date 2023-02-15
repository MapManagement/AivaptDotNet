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
            public async Task DeleteLocationType()
            {
                var selectMenuBuilder = MinecraftHelper.GetLocationTypeSelectMenu(DbContext)
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
                var selectMenuBuilder = MinecraftHelper.GetLocationTypeSelectMenu(DbContext)
                    .WithCustomId($"mc_coordinates_new,{x},{y},{z}")
                    .WithMaxValues(5);

                var component = new ComponentBuilder()
                    .WithSelectMenu(selectMenuBuilder)
                    .Build();

                await RespondAsync("What location type should be stored?", components: component);
            }

            [SlashCommand("type", "Get all coordinates that are linked to a certain location type")]
            public async Task GetCoordinatesType()
            {
                var selectMenuBuilder = MinecraftHelper.GetLocationTypeSelectMenu(DbContext)
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
                // TODO: disable select menu
                var xLong = Convert.ToUInt64(x);
                var yLong = Convert.ToUInt64(y);
                var zLong = Convert.ToUInt64(z);

                string response = MinecraftHelper
                    .InsertCoordinates(DbContext, xLong, yLong, zLong, locationTypes,
                                       Context.User.Id);

                await RespondAsync(response);
            }

            [ComponentInteraction("mc_coordinates_type", true)]
            public async Task HandleTypeCoordinatesSelectMenu(string[] locationTypes)
            {
                var message = this.Context.Interaction as SocketMessageComponent;

                if (locationTypes.Length == 0)
                    await RespondAsync("Something went wrong...");

                var customId = locationTypes[0];
                var embed = MinecraftHelper.ListCoordinatesByLocationTypeId(DbContext, customId);

                //TODO: cannot update components, needs to be set to null
                // next release will fix this
                await message.UpdateAsync(msg =>
                {
                    msg.Content = "Location type has already been chosen.";
                    msg.Components = null; 
                });

                await FollowupAsync(embed: embed);
            }

            [ComponentInteraction("mc_location_type_delete", true)]
            public async Task HandDeleteLocationTypeSelectMenu(string[] locationTypes)
            {
                var message = this.Context.Interaction as SocketMessageComponent;

                if (locationTypes.Length == 0)
                    await RespondAsync("Something went wrong...");

                var customId = locationTypes[0];
                var output = MinecraftHelper.DeleteLocationType(DbContext, customId);

                await message.UpdateAsync(msg =>
                {
                    msg.Content = "Location type has already been chosen.";
                    msg.Components = null; 
                });

                await FollowupAsync(output);

            }

            #endregion
        }

        #endregion
    }
}
