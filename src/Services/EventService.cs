using AivaptDotNet.DataClasses;
using AivaptDotNet.Helpers.General;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AivaptDotNet.Services
{


    public class EventService
    {
        #region Fields

        private IServiceProvider _services;
        private DiscordSocketClient _botClient;
        private CacheService _cacheService;
        private DatabaseService _dbService;

        #endregion

        #region Constructor

        public EventService(IServiceProvider services)
        {
            _services = services;
            _botClient = _services.GetRequiredService<DiscordSocketClient>();
            _cacheService = _services.GetRequiredService<CacheService>();
            _dbService = _services.GetRequiredService<DatabaseService>();
        }

        #endregion

        #region Public Methods

        public void AddButtonEvent(ButtonClickKeyword keyword, string key)
        {
            keyword.EventFunc = ButtonExecuted_EventAsync;
            Action clearAction = () => { _botClient.ButtonExecuted -= ButtonExecuted_EventAsync; };
            var item = new CacheKeyValue(key, keyword, DateTime.Now.AddMinutes(2), clearAction);
            _cacheService.AddKeyValue(item);
            _botClient.ButtonExecuted += ButtonExecuted_EventAsync;
        }

        #endregion

        #region Events

        private async Task ButtonExecuted_EventAsync(SocketMessageComponent arg)
        {
            ulong userId = arg.User.Id;
            ulong messageId = arg.Message.Id;
            var buttonId = arg.Data.CustomId;

            var keyValue = _cacheService.GetKeyValue(messageId.ToString()) as CacheKeyValue;
            var keywords = keyValue?.Value as ButtonClickKeyword;
            if (keywords == null) return;

            string commandName = keywords.Parameters["name"] as string;

            if (messageId == keywords.BotReplyMsgId && (userId == keywords.InitialUserId))
            {
                if (buttonId == $"del-{keywords.InitialMsgId}")
                {
                    string sql = @"delete from simple_command where name = @NAME and creator = @CREATOR";
                    var param = new Dictionary<string, object>();
                    param.Add("@NAME", commandName);
                    param.Add("@CREATOR", keywords.InitialUserId.ToString());

                    _dbService.ExecuteDML(sql, param);

                    await arg.UpdateAsync(x =>
                    {
                        x.Embed = SimpleEmbed.MinimalEmbed("Confirmation")
                            .WithDescription("Command has been deleted!")
                            .WithFooter(arg.User.Username, arg.User.GetAvatarUrl())
                            .Build();

                        var buttons = new List<ButtonBuilder>()
                        {
                            { new ButtonBuilder("Delete", $"del-dis", ButtonStyle.Danger, isDisabled: true) },
                            { new ButtonBuilder("Cancel", $"cancel-dis", ButtonStyle.Secondary, isDisabled: true) }
                        };
                        var buttonComponent = SimpleComponents.MultipleButtons(buttons);
                        x.Components = buttonComponent.Build();
                    });

                    _botClient.ButtonExecuted -= ButtonExecuted_EventAsync;
                }
                else if (buttonId == $"cancel-{keywords.InitialMsgId}")
                {
                    await arg.UpdateAsync(x =>
                    {
                        x.Embed = SimpleEmbed.MinimalEmbed("Confirmation")
                            .WithDescription("Cancelled!")
                            .WithFooter(arg.User.Username, arg.User.GetAvatarUrl())
                            .Build();

                        var buttons = new List<ButtonBuilder>()
                        {
                            { new ButtonBuilder("Delete", $"del-dis", ButtonStyle.Danger, isDisabled: true) },
                            { new ButtonBuilder("Cancel", $"cancel-dis", ButtonStyle.Secondary, isDisabled: true) }
                        };
                        var buttonComponent = SimpleComponents.MultipleButtons(buttons);
                        x.Components = buttonComponent.Build();
                    });

                    _botClient.ButtonExecuted -= ButtonExecuted_EventAsync;
                }
            }
        }

        #endregion
    }
}