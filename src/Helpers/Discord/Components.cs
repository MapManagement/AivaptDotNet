using System.Collections.Generic;
using System.IO;
using Discord;
using Discord.WebSocket;

namespace AivaptDotNet.Helpers.Discord
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

    public static class SimpleComponents
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

        public static SelectMenuBuilder DisabledSelectMenu(string placeholder)
        {
            var selectMenu = new SelectMenuBuilder()
            {
                CustomId = "disabled_select_menu",
                Placeholder = placeholder,
                IsDisabled = true,
                MinValues = 1,
                MaxValues = 1
            };

            selectMenu.AddOption("Empty", "empty_option");

            return selectMenu;
        }

        public static async void DisableSelectMenuMessage(SocketMessageComponent message,
                                                          string content)
        {
            var disabledSelectMenu = SimpleComponents.DisabledSelectMenu("Disabled");
            var messageComponent = new ComponentBuilder()
                .WithSelectMenu(disabledSelectMenu)
                .Build();

            await message.UpdateAsync(msg =>
            {
                msg.Content = content;
                msg.Components = messageComponent;
            });
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
