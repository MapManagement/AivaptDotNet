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
            builder.WithThumbnailUrl("attachment://" + resourcePath); //TODO: image is not displayed

            return builder.Build();
        }
    }

    public class SimpleComponents
    {
        public static ComponentBuilder MultipleButtons(List<ButtonBuilder> buttons)
        {
            ComponentBuilder builder = new ComponentBuilder();

            foreach (var button in buttons)
            {
                builder.WithButton(button);
            }

            return builder;
        }

        public static ComponentBuilder MultipleChoice(string identifier, string placeholder, List<UserOption> options)
        {
            SelectMenuBuilder choiceBuilder = new SelectMenuBuilder(identifier, null, placeholder);

            foreach (var option in options)
            {
                choiceBuilder.AddOption(option.Label, option.Identifier, option.Description);
            }

            return new ComponentBuilder().WithSelectMenu(choiceBuilder);
        }

        public static void DisableComponent()
        {
            return;
        }

        public static void DisableMessageComponents(IReadOnlyCollection<ActionRowComponent> components)
        {
            foreach (var row in components)
            {
                foreach (var component in row.Components)
                {
                    if (component.Type == ComponentType.Button)
                    {
                        var button = component as ButtonComponent;
                        var builder = button.ToBuilder();
                        builder.IsDisabled = true;
                        builder.Build();
                    }
                    else if (component.Type == ComponentType.SelectMenu)
                    {
                         var selectMenu = component as SelectMenuComponent;
                         var builder = selectMenu.ToBuilder();
                         builder.IsDisabled = true;
                         builder.Build();
                    }
                }
            }
        }
    }

    public class UserOption
    {
        public string Identifier;
        public string Label;
        public string Description;

        public UserOption(string identifier, string label, string description)
        {
            Identifier = identifier;
            Label = label;
            Description = description;
        }
    }
}