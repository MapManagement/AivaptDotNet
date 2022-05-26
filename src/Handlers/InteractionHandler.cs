using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System;

using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using AivaptDotNet.Helpers.General;
using AivaptDotNet.Services;
using Discord.Interactions;

namespace AivaptDotNet.Handlers
{
    public class InteractionHandler
    {
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _botClient;
        private readonly InteractionService _interactionService;
        private readonly DatabaseService _dbService;

        public InteractionHandler(IServiceProvider services)
        {
            _services = services;
            _botClient = services.GetRequiredService<DiscordSocketClient>();
            _interactionService = services.GetRequiredService<InteractionService>();
            _dbService = services.GetRequiredService<DatabaseService>();
        }

        public async Task InitializeCommands()
        {
            _botClient.InteractionCreated += HandleInteraction;
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), services: _services);
        }

        private async Task HandleInteraction(SocketInteraction interaction) 
        {
            if(interaction == null)
                return;

            var context = new SocketInteractionContext(_botClient, interaction);

            try 
            {
                await _interactionService.ExecuteCommandAsync(context, _services);

            }
            catch(Exception e)
            {
                Embed errorEmbed = SimpleEmbed.ErrorEmbed(e.InnerException.ToString().Substring(0, 256));
                await context.Channel.SendMessageAsync("", false, errorEmbed);
            }
           
        }
    }
}