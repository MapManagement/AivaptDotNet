using System;
using System.Threading.Tasks;
using System.IO;

using Discord.Commands;


namespace AivaptDotNet.Modules 
{
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [Summary("Simple Test-Command")]
        public async Task TestCommand()
        {
            await ReplyAsync("Test succeeded!");
        }
    }
}