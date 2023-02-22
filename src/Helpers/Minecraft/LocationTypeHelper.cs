using AivaptDotNet.Helpers.Discord;
using AivaptDotNet.Services.Database;
using AivaptDotNet.Services.Database.Models;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AivaptDotNet.Helpers.Minecraft
{
    public static class LocationTypeHelper
    {
        #region Methods

        #region Public

        public static string InsertLocationType(BotDbContext dbContext, string locationName)
        {
            var location = new McLocation
            {
                LocationName = locationName
            };

            dbContext.McLocations.Add(location);
            dbContext.SaveChanges();

            return $"A new location type called \"{locationName}\" has been created.";
        }

        public static string DeleteLocationType(BotDbContext dbContext, string customId)
        {
            var locationType = ExtractLocationTypeFromId(dbContext, customId);

            if (locationType == null)
                return $"There is no location called \"{locationType.LocationName}\".";

            if (AreCoordinatesLinked(dbContext, locationType))
                return $"There are still coordinates linked to this type.";

            dbContext.Remove(locationType);
            dbContext.SaveChanges();

            return $"The location type called \"{locationType.LocationName}\" has been deleted.";
        }

        public static List<McLocation> GetAllLocationTypes(BotDbContext dbContext)
        {
            return dbContext.McLocations.ToList();
        }

        public static Embed ListLocationTypes(BotDbContext dbContext)
        {
            var locationTypes = GetAllLocationTypes(dbContext);

            var embedContent = new StringBuilder();
            foreach (var lt in locationTypes)
            {
                embedContent.Append($"{lt.LocationId} - {lt.LocationName}\n");
            }

            var embed = SimpleEmbed.MinimalEmbed(embedTitle: "Minecraft Location Types",
                                                 embedDescription: embedContent.ToString());

            return embed.Build();
        }

        
        public static SelectMenuBuilder GetLocationTypeSelectMenu(BotDbContext dbContext)
        {
            var locationTypes = GetAllLocationTypes(dbContext);

            var selectMenuBuilder = new SelectMenuBuilder()
            {
                Placeholder = "Select a Minecraft location type...",
                MinValues = 1,
            };

            foreach (var locationType in locationTypes)
            {
                var optionId = $"{locationType.LocationId}";
                selectMenuBuilder.AddOption(locationType.LocationName, optionId);
            }

            return selectMenuBuilder;
        }

        public static McLocation ExtractLocationTypeFromId(BotDbContext dbContext, string customId)
        {
            uint parsedCustomId;
            bool success = uint.TryParse(customId, out parsedCustomId);
            
            if (!success)
                return null;

            McLocation mcLocation = dbContext.McLocations.Find(parsedCustomId);

            return mcLocation;
        }


        #endregion

        #region Private

        private static bool AreCoordinatesLinked(BotDbContext dbContext, McLocation location)
        {
            // TODO: int instead of uint?
            uint locationId = location.LocationId;

            bool areLinked = dbContext.McCoordinates
                .Any(mc => mc.Locations.Any(l => l.LocationId == locationId));

            return areLinked;
        }

        #endregion

        #endregion
    }
}
