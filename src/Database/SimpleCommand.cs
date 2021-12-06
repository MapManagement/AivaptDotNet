using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace AivaptDotNet.Database
{
    public class SimpleCommand
    {
        public SimpleCommand(string name, string text, string title, bool active, string creator)
        {
            Name = name;
            Text = text;
            Title = title;
            Active = active;
            Creator = creator;
            Color = "#1ABC9C";
        }

        public SimpleCommand(string name, string text, string title, bool active, string creator, string color)
        {
            Name = name;
            Text = text;
            Title = title;
            Active = active;
            Creator = creator;
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

        private string _creator;
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        private string _color;
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

    }

}