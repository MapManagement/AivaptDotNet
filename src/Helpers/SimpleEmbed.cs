using System.Collections.Generic;

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


        public static EmbedBuilder FieldsEmbed(string embedTitle, Dictionary<string, string> embedFields)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle(embedTitle);

            foreach (var item in embedFields)
            {
                builder.AddField(item.Key, item.Value);
            }

            return builder;
        }

        public static EmbedBuilder FieldsEmbed(string embedTitle, string embedDescription, Dictionary<string, string> embedFields)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle(embedTitle);
            builder.WithDescription(embedDescription);

            foreach (var item in embedFields)
            {
                builder.AddField(item.Key, item.Value);
            }

            return builder;
        }

        public static Embed ErrorEmbed(string errorText)
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Error");
            builder.WithDescription("An error occured.");
            builder.AddField("Details", errorText);
            builder.WithColor(Color.Red);
            builder.WithThumbnailUrl(""); //TODO: add URL
            
            return builder.Build();
        }


    }
}