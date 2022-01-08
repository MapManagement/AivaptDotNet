using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace AivaptDotNet.Database
{
    public class SimpleCommand
    {
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

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        private ulong _creatorId;
        public ulong CreatorId
        {
            get { return _creatorId; }
            set { _creatorId = value; }
        }

        private string _color;
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

    }

    public class Role
    {
        public Role(ulong roleId, ulong guildId, bool modPermissions)
        {
            RoleId = roleId;
            GuildId = guildId;
            ModPermissions = modPermissions;
        }

        private ulong _roleId;
        public ulong RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }

        private ulong _guildId;
        public ulong GuildId
        {
            get { return _guildId; }
            set { _guildId = value; }
        }
        
        private bool _modPermissions;
        public bool ModPermissions
        {
            get { return _modPermissions; }
            set { _modPermissions = value; }
        }
    }

    public class Guild
    {
        public Guild(ulong id, string name, ulong ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
        }

        private ulong _id;
        public ulong Id
        {
            get { return _id; }
            set { _id = value; } 
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        private ulong _ownerId;
        public ulong OwnerId
        {
            get { return _ownerId; }
            set { _ownerId = value; }
        }
    }
}