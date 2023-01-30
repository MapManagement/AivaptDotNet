using AivaptDotNet.Helpers.DiscordClasses;
using AivaptDotNet.Services.Database;
using AivaptDotNet.Services.Database.Models;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AivaptDotNet.Helpers.Modules
{
    public static class MinecraftHelper
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

        public static string DeleteLocationType(BotDbContext dbContext, string locationName)
        {
            var location = GetLocationByName(dbContext, locationName);

            if (location == null || AreCoordinatesLinked(dbContext, location))
                return $"There is no location called\"{locationName}\".";

            dbContext.Remove(location);
            dbContext.SaveChanges();

            return $"The location type called \"{locationName}\" has been deleted.";
        }

        public static List<McLocation> GetAllLocationTypes(BotDbContext dbContext)
        {
            return dbContext.McLocations.ToList();
        }

        public static Embed ListLocationTypes(BotDbContext dbContext)
        {
            var locationTypes = GetAllLocationTypes(dbContext);
            var embed = SimpleEmbed.MinimalEmbed("Minecraft Location Types");

            var embedContent = new StringBuilder();
            foreach (var lt in locationTypes)
            {
                embedContent.Append($"{lt.LocationName} - {lt.LocationId}\n");
            }

            return embed.Build();
        }

        public static string InsertCoordinates(BotDbContext dbContext, ulong x, ulong y, ulong z,
                                               List<McLocation> mcLocations, ulong submitterId)
        {
            var coordinates = new McCoordinates
            {
                X = x,
                Y = y,
                Z = z,
                Locations = mcLocations,
                SubmitterId = submitterId
            };

            dbContext.McCoordinates.Add(coordinates);
            dbContext.SaveChanges();

            string baseText = $"New coordinates at X: {x}, Y: {y}, Z: {z} have been linked with " +
                               "following location types: ";
            var stringBuilder = new StringBuilder(baseText);

            foreach (var mcLocation in mcLocations)
            {
                stringBuilder
                    .Append(mcLocation.LocationName)
                    .Append(", ");
            }

            return stringBuilder.ToString();
        }

        public static string DeleteCoordinates(BotDbContext dbContext, int coordinatesId)
        {
            var coordinates = dbContext.McCoordinates.Find(coordinatesId);

            if (coordinates == null)
                return $"There are no coordinates with the ID \"{coordinatesId}\".";

            dbContext.Remove(coordinates);
            dbContext.SaveChanges();

            return $"The coordinates with the ID \"{coordinatesId}\" have been deleted.";
        }

        #endregion

        #region Private

        private static McLocation GetLocationByName(BotDbContext dbContext, string locationName)
        {
            var location = dbContext.McLocations.First(ml => ml.LocationName == locationName);

            return location;
        }

        private static bool ExistsLocationName(BotDbContext dbContext, string locationName)
        {
            bool locationExists = dbContext.McLocations.Any(ml => ml.LocationName == locationName);

            return locationExists;
        }

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
