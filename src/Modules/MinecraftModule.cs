using AivaptDotNet.Helpers.Modules;
using AivaptDotNet.Services.Database;
using Discord;
using Discord.Interactions;
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
                var customId = $"mc-coordinates-select,{x},{y},{z}";

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

            #endregion

            #region Component Interaction

            [ComponentInteraction("mc-coordinates-select,*,*,*", true)]
            public async Task HandleCoordinatesSelectMenu(string x, string y, string z, 
                                                          string[] locationTypes)
            {
                var xLong = Convert.ToUInt64(x); 
                var yLong = Convert.ToUInt64(y); 
                var zLong = Convert.ToUInt64(z);
                
                string response = MinecraftHelper
                    .InsertCoordinates(DbContext, xLong, yLong, zLong, locationTypes,
                                       Context.User.Id);

                await RespondAsync(response);
            }

            #endregion
        }

        #endregion
    }
}
