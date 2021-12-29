using System;
using System.Collections.Generic;

namespace AivaptDotNet.AivaptClases
{
    public class ReactionKeywords
    {

        public ReactionKeywords(ulong userMessageId, ulong botMessageId, ulong authorId, Dictionary<string, object> parameters)
        {   
            UserMessageId = userMessageId;
            BotMessageId = botMessageId;
            AuthorId = authorId;
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
        public Dictionary<string, object> Parameters;
    }
}