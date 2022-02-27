using System;
using System.IO;

namespace AivaptDotNet.Helpers
{
    public class Credentials
    {
        public ulong AdminId { get; set; }

        public string BotToken { get; set; }

        public string DbConnectionString { get; set; }

        public string LavalinkPassword { get; set; }
    }
}