using Discord;

namespace AivaptDotNet.Helpers.Discord
{
    public class DefaultActivity : IActivity
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _details;
        public string Details
        {
            get { return _details; }
            set { _details = value; }
        }

        private ActivityProperties _flags;
        public ActivityProperties Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        private ActivityType _type;
        public ActivityType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public DefaultActivity(string name, string details)
        {
            Name = name;
            Details = details;
            Flags = ActivityProperties.None;
            Type = ActivityType.CustomStatus;
        }
    }

    public class GameActivity : DefaultActivity
    {
        public GameActivity(string name, string details) : base(name, details)
        {
            Type = ActivityType.Playing;
        }
    }

    public class AudioActivity : DefaultActivity
    {
        public AudioActivity(string name, string details) : base(name, details)
        {
            Type = ActivityType.Listening;
        }
    }
}
