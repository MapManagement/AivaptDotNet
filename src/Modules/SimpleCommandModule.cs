using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

namespace AivaptDotNet.Modules 
{
    public class SimpleCommandModule : ModuleBase<AivaptCommandContext>
    {

        [Command("create")]
        [Summary("Creates new simple command")]
        public async Task CreateCmdCommand(string name, string text, string title = "")
        {
            string creator = Context.User.Id.ToString();

            string sql = @"insert into simple_command (name, command_text, title, active, creator) values (@NAME, @TEXT, @TITLE, 1, @CREATOR)";
            var param = new Dictionary<string, object>();
            param.Add("@NAME", name);
            param.Add("@TEXT", text);
            param.Add("@TITLE", title);
            param.Add("@CREATOR", creator);

            Context._dbConnector.ExecuteDML(sql, param);

            await Context.Channel.SendMessageAsync("Success!");
        }
    }
}