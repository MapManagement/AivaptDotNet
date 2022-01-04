using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace AivaptDotNet.AivaptClases
{

    public class CacheKeyword
    {
        public CacheKeyword(Dictionary<string, object> parameters)
        {
            Parameters = parameters;
            _raisedAt = DateTime.Now;
        }

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
    }

    public class EventKeyword : CacheKeyword
    {
        public EventKeyword(ulong initialMsgId, ulong botReplyMsgId, ulong initialUserId, Dictionary<string, object> parameters) : base(parameters)
        {
            _initialMsgId = initialMsgId;
            _botReplyMsgId = botReplyMsgId;
            _initialUserId = initialUserId;
        }

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
    }

    public class ButtonClickKeyword : EventKeyword
    {
        public ButtonClickKeyword(ulong initialMsgId, ulong botReplyMsgId, ulong initialUserId, Func<SocketMessageComponent, Task> eventFunc, Dictionary<string, object> parameters) :
            base(initialMsgId, botReplyMsgId, initialUserId, parameters)
        {
            _eventFunc = eventFunc;
        }

        private Func<SocketMessageComponent, Task> _eventFunc;
        public Func<SocketMessageComponent, Task> EventFunc
        {
            get { return _eventFunc; }
        }
    }

    public class ReactionAddKeyword : EventKeyword
    {
        public ReactionAddKeyword(ulong initialMsgId,
                                  ulong botReplyMsgId,
                                  ulong initialUserId,
                                  Func<Cacheable<IUserMessage, ulong>, Cacheable<IMessageChannel, ulong>, SocketReaction, Task> eventFunc, Dictionary<string, object> parameters) :
            base(initialMsgId, botReplyMsgId, initialMsgId, parameters)
        {
            _eventFunc = eventFunc;
        }

        private Func<Cacheable<IUserMessage, ulong>, Cacheable<IMessageChannel, ulong>, SocketReaction, Task> _eventFunc;
        public Func<Cacheable<IUserMessage, ulong>, Cacheable<IMessageChannel, ulong>, SocketReaction, Task> EventFunc
        {
            get { return _eventFunc; }
        }
    }
}