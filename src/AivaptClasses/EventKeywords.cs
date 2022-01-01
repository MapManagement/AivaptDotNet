using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord.WebSocket;

namespace AivaptDotNet.AivaptClases
{
    public enum EventType
    {
        ButtonClick,
        ReactionAdd,
        Reply,
        SelectMenu
    }

    public class ReactionKeywords //TODO: inheritance?
    {

        public ReactionKeywords(ulong userMessageId, ulong botMessageId, ulong authorId, Dictionary<string, object> parameters)
        {   
            UserMessageId = userMessageId;
            BotMessageId = botMessageId;
            AuthorId = authorId;
            Parameters = parameters;
            _raisedAt = DateTime.Now;
        }

        public ReactionKeywords(ulong userMessageId, ulong botMessageId, ulong authorId, Func<SocketMessageComponent, Task> eventFunc, EventType eType,Dictionary<string, object> parameters)
        {   
            UserMessageId = userMessageId;
            BotMessageId = botMessageId;
            AuthorId = authorId;
            EventFunc = eventFunc;
            EType = eType;
            Parameters = parameters;
            _raisedAt = DateTime.Now;
        }

        private DateTime _raisedAt;
        public DateTime RaisedAt
        {
            get { return _raisedAt; }
        }

        public ulong UserMessageId;
        public ulong BotMessageId;
        public ulong AuthorId;
        public Func<SocketMessageComponent, Task> EventFunc;
        public Dictionary<string, object> Parameters;
        public EventType EType;
    }
}