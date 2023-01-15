using System.Threading.Tasks;
using System.Reflection;
using System;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using AivaptDotNet.Helpers.DiscordClasses;
using AivaptDotNet.Services;
using AivaptDotNet.Services.Database;

namespace AivaptDotNet.Handlers
{
    public class InteractionHandler
    {
		#region Fields

        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _botClient;
        private readonly InteractionService _interactionService;
        private readonly BotDbContext _dbContext;

		#endregion

		#region Constructor

        public InteractionHandler(IServiceProvider services)
        {
            _services = services;
            _botClient = services.GetRequiredService<DiscordSocketClient>();
            _interactionService = services.GetRequiredService<InteractionService>();
            _dbContext = services.GetRequiredService<BotDbContext>();
        }

		#endregion

		#region Methods

		#region Public

        public async Task InitializeCommands()
        {
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), services: _services);

            _botClient.InteractionCreated += HandleInteraction;
        }

		#endregion

		#region Private

        private async Task HandleInteraction(SocketInteraction interaction) 
        {
            if (interaction == null)
                return;

            var context = new SocketInteractionContext(_botClient, interaction);

            try 
            {
                await _interactionService.ExecuteCommandAsync(context, _services);
            }
            catch (Exception e)
            {
                Embed errorEmbed = SimpleEmbed.ErrorEmbed(e.InnerException.ToString().Substring(0, 256));
                await context.Channel.SendMessageAsync("", false, errorEmbed);
            }
        }

		#endregion

		#endregion 
    }
}
