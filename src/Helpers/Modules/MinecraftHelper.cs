using AivaptDotNet.Helpers.DiscordClasses;
using AivaptDotNet.Services.Database;
using AivaptDotNet.Services.Database.Models;
using Discord;
using System;
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

        public static Embed ListCoordinatesByLocationType(BotDbContext dbContext, McLocation locationType)
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
            var locationType = ExtractLocationTypeFromId(dbContext, customId);
            var embed = ListCoordinatesByLocationType(dbContext, locationType);

            return embed;
        }

        public static Modal GetNewCoordinatesModal(BotDbContext dbContext)
        {
            var locationTypes = GetAllLocationTypes(dbContext);

            var modal = new ModalBuilder()
                .WithCustomId("mc-new-coordinates-modal")
                .WithTitle("New Coordinates")
                .AddTextInput("x", "x", placeholder: "0", required: true)
                .AddTextInput("y", "y", placeholder: "0", required: true)
                .AddTextInput("z", "z", placeholder: "0")
                .AddTextInput("Description", "description", TextInputStyle.Paragraph)
                .Build();

            return modal;
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
                var optionId = $"mc_location_type,{locationType.LocationId}";
                selectMenuBuilder.AddOption(locationType.LocationName, optionId);
            }

            return selectMenuBuilder;
        }

        #endregion

        #region Private

        private static bool ExistsLocationTyepName(BotDbContext dbContext, string locationName)
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

        private static List<McLocation> ExtractLocationTypesFromIds(BotDbContext dbContext,
                                                                  string[] locationTypes)
        {
            List<McLocation> mcLocations = new List<McLocation>();

            foreach (var customId in locationTypes)
            {
                mcLocations.Add(ExtractLocationTypeFromId(dbContext, customId));
            }

            return mcLocations;
        }

        private static McLocation ExtractLocationTypeFromId(BotDbContext dbContext, string customId)
        {
            var parsedId = Convert.ToUInt32(customId.Split(",")[1]);
            McLocation mcLocation = dbContext.McLocations.Find(parsedId);

            return mcLocation;
        }

        private static List<ulong> ExtractCoordinatesFromString(string coordinatesString)
        {
            List<ulong> coordinates = new List<ulong>();
            var splittedStrings = coordinatesString.Split(",");

            for (int i = 1; i < splittedStrings.Length; i++)
            {
                var parsedCoordinate = Convert.ToUInt64(splittedStrings[i]);
                coordinates.Add(parsedCoordinate);
            }

            return coordinates;
        }

        #endregion

        #endregion
    }
}
