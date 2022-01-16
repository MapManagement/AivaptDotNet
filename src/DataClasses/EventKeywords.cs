using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace AivaptDotNet.DataClasses
{
    #region CacheKeyword Class

    public class CacheKeyword
    {
        #region Constructor

        public CacheKeyword(Dictionary<string, object> parameters)
        {
            Parameters = parameters;
            _raisedAt = DateTime.Now;
        }

        #endregion

        #region Properties

        private DateTime _raisedAt;
        public DateTime RaisedAt
        {
            get { return _raisedAt; }
        }

        private Dictionary<string, object> _parameters;
        public Dictionary<string, object> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        #endregion
    }

    #endregion

    #region EventKeyword Class

    public class EventKeyword : CacheKeyword
    {
        #region Constructors

        public EventKeyword(ulong initialMsgId, ulong botReplyMsgId, ulong initialUserId, Dictionary<string, object> parameters) : base(parameters)
        {
            _initialMsgId = initialMsgId;
            _botReplyMsgId = botReplyMsgId;
            _initialUserId = initialUserId;
        }

        #endregion

        #region Properties

        private ulong _initialMsgId;
        public ulong InitialMsgId
        {
            get { return _initialMsgId; }
        }

        private ulong _botReplyMsgId;
        public ulong BotReplyMsgId
        {
            get { return _botReplyMsgId; }
        }

        private ulong _initialUserId;
        public ulong InitialUserId
        {
            get { return _initialUserId; }
        }

        #endregion
    }

    #endregion

    #region ButtonClickKeyword Class

    public class ButtonClickKeyword : EventKeyword
    {
        #region Constructors

        public ButtonClickKeyword(ulong initialMsgId, ulong botReplyMsgId, ulong initialUserId, Dictionary<string, object> parameters) :
            base(initialMsgId, botReplyMsgId, initialUserId, parameters)
        {

        }

        #endregion

        #region Properties

        private Func<SocketMessageComponent, Task> _eventFunc;
        public Func<SocketMessageComponent, Task> EventFunc
        {
            get { return _eventFunc; }
            set { _eventFunc = value; }
        }

        #endregion
    }

    #endregion

    #region ReactionAddKeyword Class

    public class ReactionAddKeyword : EventKeyword
    {
        #region Constructor

        public ReactionAddKeyword(ulong initialMsgId,
                                  ulong botReplyMsgId,
                                  ulong initialUserId,
                                  Func<Cacheable<IUserMessage, ulong>, Cacheable<IMessageChannel, ulong>, SocketReaction, Task> eventFunc, Dictionary<string, object> parameters) :
            base(initialMsgId, botReplyMsgId, initialMsgId, parameters)
        {
            _eventFunc = eventFunc;
        }

        #endregion

        #region Properties

        private Func<Cacheable<IUserMessage, ulong>, Cacheable<IMessageChannel, ulong>, SocketReaction, Task> _eventFunc;
        public Func<Cacheable<IUserMessage, ulong>, Cacheable<IMessageChannel, ulong>, SocketReaction, Task> EventFunc
        {
            get { return _eventFunc; }
        }

        #endregion
    }

    #endregion
}