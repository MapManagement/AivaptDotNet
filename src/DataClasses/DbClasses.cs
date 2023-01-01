using System;


namespace AivaptDotNet.DataClasses
{
    #region SimpleCommand Class

    public class SimpleCommand
    {
        #region Constructors

        public SimpleCommand(string name, string text, string title, bool active, ulong creatorId)
        {
            Name = name;
            Text = text;
            Title = title;
            Active = active;
            CreatorId = creatorId;
            Color = "#1ABC9C";
        }

        public SimpleCommand(string name, string text, string title, bool active, ulong creatorId, string color)
        {
            Name = name;
            Text = text;
            Title = title;
            Active = active;
            CreatorId = creatorId;
            Color = color;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }

        public bool Active { get; set; }

        public ulong CreatorId { get; set; }

        public string Color { get; set; }

        #endregion
    }

    #endregion

    #region Role Class

    public class Role
    {
        #region Constructors

        public Role(ulong roleId, ulong guildId, bool modPermissions)
        {
            RoleId = roleId;
            GuildId = guildId;
            ModPermissions = modPermissions;
        }

        #endregion

        #region Properties

        public ulong RoleId { get; set; }

        public ulong GuildId { get; set; }
        
        public bool ModPermissions { get; set; }

        #endregion
    }

    #endregion

    #region Guild Class

    public class Guild
    {
        #region Constructors

        public Guild(ulong id, string name, ulong ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
        }

        #endregion

        #region Properties

        public ulong Id { get; set; }

        public string Name { get; set; }
        
        public ulong OwnerId { get; set; }

        #endregion
    }

    #endregion

    #region Quote Class

    public class Quote
    {
        #region Constructors

        public Quote(ulong id, ulong userId, string text, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            Text = text;
            CreatedAt = createdAt;
        }

        #endregion

        public ulong Id { get; set; }

        public ulong UserId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    #endregion
}
