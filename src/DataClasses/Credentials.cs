namespace AivaptDotNet.DataClasses
{
    public class Credentials
    {
        public ulong AdminId { get; set; }

        public string BotToken { get; set; }

        public string DbConnectionString { get; set; }

        public string LavalinkPassword { get; set; }

        public ulong? DebugGuildId { get; set; }
    }
}
