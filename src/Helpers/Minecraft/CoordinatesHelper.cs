using AivaptDotNet.Helpers.Discord;
using AivaptDotNet.Services.Database;
using AivaptDotNet.Services.Database.Models;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AivaptDotNet.Helpers.Minecraft
{
    public static class CoordinatesHelper
    {
        #region Methods

        #region Public

        public static Embed ListCoordinatesByLocationType(BotDbContext dbContext,
                                                          McLocation locationType)
        {
            var coordinates = dbContext.McCoordinates
                .Where(x => x.Locations.Contains(locationType));

            string embedTitle = $"Coordinates including '{locationType.LocationName}'";
            var fields = new List<EmbedFieldBuilder>();

            foreach (var coordinate in coordinates)
            {
                var field = new EmbedFieldBuilder()
                {
                    Name = coordinate.CoordinatesId.ToString(),
                    Value = $"X: {coordinate.X} Y: {coordinate.Y}, Z: {coordinate.Z}",
                    IsInline = true
                };

                fields.Add(field);
            }

            var embed = SimpleEmbed.FieldsEmbed(embedTitle, fields);
            return embed.Build();
        }

        public static string InsertCoordinates(BotDbContext dbContext, ulong x, ulong y, ulong z,
                                               string[] locationTypes, ulong submitterId)
        {
            var mcLocations = ExtractLocationTypesFromIds(dbContext, locationTypes);

            var mcCoordinates = new McCoordinates
            {
                X = x,
                Y = y,
                Z = z,
                Locations = mcLocations,
                SubmitterId = submitterId
            };

            dbContext.McCoordinates.Add(mcCoordinates);
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

        public static Embed ListCoordinatesByLocationTypeId(BotDbContext dbContext, string customId)
        {
            var locationType = LocationTypeHelper.ExtractLocationTypeFromId(dbContext, customId);
            var embed = ListCoordinatesByLocationType(dbContext, locationType);

            return embed;
        }

        #endregion

        #region Private

        private static List<McLocation> ExtractLocationTypesFromIds(BotDbContext dbContext,
                                                                  string[] locationTypes)
        {
            List<McLocation> mcLocations = new List<McLocation>();

            foreach (var customId in locationTypes)
            {
                mcLocations.Add(LocationTypeHelper.ExtractLocationTypeFromId(dbContext, customId));
            }

            return mcLocations;
        }
        
        #endregion

        #endregion
    }
}
