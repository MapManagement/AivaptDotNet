using Discord;
using Discord.Interactions;

namespace AivaptDotNet.Helpers.SimpleCommands
{
    public class NewCommandModal : IModal
    {
        public string Title => "New Simple Command";

        [RequiredInput(true)]
        [InputLabel("Name")]
        [ModalTextInput("new_name", style: TextInputStyle.Short)]
        public string CommandName { get; set; }

        [InputLabel("Title")]
        [ModalTextInput("new_title", style: TextInputStyle.Short)]
        public string CommandTitle { get; set; }

        [InputLabel("Text")]
        [ModalTextInput("new_text", style: TextInputStyle.Paragraph)]
        public string CommandText { get; set; }

        [RequiredInput(true)]
        [InputLabel("Display Color")]
        [ModalTextInput("new_color", style: TextInputStyle.Short)]
        public string CommandColor { get; set; }
    }

    public class EditCommandModal : IModal
    {
        public string Title => "Edit Simple Command";

        [RequiredInput(true)]
        [InputLabel("Name")]
        [ModalTextInput("new_name", style: TextInputStyle.Short)]
        public string CommandName { get; set; }

        [InputLabel("Title")]
        [ModalTextInput("new_title", style: TextInputStyle.Short)]
        public string CommandTitle { get; set; }

        [InputLabel("Text")]
        [ModalTextInput("new_text", style: TextInputStyle.Paragraph)]
        public string CommandText { get; set; }

        [RequiredInput(true)]
        [InputLabel("Display Color")]
        [ModalTextInput("new_color", style: TextInputStyle.Short)]
        public string CommandColor { get; set; }
    }

}
