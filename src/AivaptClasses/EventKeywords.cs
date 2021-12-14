using System;
using System.Collections.Generic;

namespace AivaptDotNet.AivaptClases
{
    public class ReactionKeywords
    {

        public ReactionKeywords(ulong originMessageId, ulong authorId, Dictionary<string, object> parameters)
        {
            OriginMessageId = originMessageId;
            AuthorId = authorId;
            Parameters = parameters;
            _raisedAt = DateTime.Now;
        }

        private DateTime _raisedAt;
        public DateTime RaisedAt
        {
            get { return _raisedAt; }
        }

        public ulong OriginMessageId;
        public ulong AuthorId;
        public Dictionary<string, object> Parameters;
    }
}