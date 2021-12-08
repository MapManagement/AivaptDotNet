using System.Collections.Generic;
using System.IO;

using Discord;

namespace AivaptDotNet.Helpers
{
    public static class SimpleEmbed
    {
        public static EmbedBuilder MinimalEmbed(string embedTitle)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle(embedTitle);

            return builder;
        }

        public static EmbedBuilder MinimalEmbed(string embedTitle, string embedDescription)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle(embedTitle);
            builder.WithDescription(embedDescription);

            return builder;
        }


        public static EmbedBuilder FieldsEmbed(string embedTitle, List<EmbedFieldBuilder> embedFields)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle(embedTitle);

            foreach (var item in embedFields)
            {
                builder.AddField(item);
            }

            return builder;
        }

        public static EmbedBuilder FieldsEmbed(string embedTitle, string embedDescription, List<EmbedFieldBuilder> embedFields)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle(embedTitle);
            builder.WithDescription(embedDescription);

            foreach (var item in embedFields)
            {
                builder.AddField(item);
            }

            return builder;
        }

        public static Embed ErrorEmbed(string errorText)
        {
            string cwd = Directory.GetCurrentDirectory();
            string resourcePath = cwd + "/Resources/Images/error_1.png";

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Error");
            builder.WithDescription("An error occured.");
            builder.AddField("Details", errorText);
            builder.WithColor(Color.Red);
            builder.WithThumbnailUrl("attachment://" + resourcePath); //TODO: iamge is not displayed
            
            return builder.Build();
        }


    }
}