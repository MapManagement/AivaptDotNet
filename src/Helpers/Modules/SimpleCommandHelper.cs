using System;
using System.Collections.Generic;
using AivaptDotNet.Services;
using Discord;

namespace AivaptDotNet.Helpers.Modules
{
    public static class SimpleCommandHelper
    {
        #region Methods

        #region Public Methods

        public static void GetSimpleCommand()
        {

        }

        public static Dictionary<string, ulong> GetAllSimpleCommands(DatabaseService dbService)
        {
            string sql = @"select name, creator_id from simple_command";
            Dictionary<string, ulong> commands = new Dictionary<string, ulong>();

            using (var reader = dbService.ExecuteSelect(sql))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        commands.Add(reader.GetString("name"), reader.GetUInt64("creator_id"));
                    }
                }
            }
            
            return commands;
        }

        //TODO: add operation result
        public static void CreateSimpleCommand(DatabaseService dbService, string name, string text, ulong authorId)
        {
            string sql = @"insert into simple_command (name, command_text, title, active, creator) values (@NAME, @TEXT, @TITLE, 1, @CREATOR)";
            var param = new Dictionary<string, object>()
            {
                { "@NAME", name },
                { "@TEXT", text },
                { "@TITLE", name },
                { "@CREATOR_ID", authorId.ToString() }
            };

            dbService.ExecuteDML(sql, param);
        }

        public static void EditSimpleCommand(DatabaseService dbService, string name, string newText)
        {
            string sql = @"update simple_command set command_text = @TEXT where name = @NAME";
            var param = new Dictionary<string, object>()
            {
                { "@NAME", name },
                { "@TEXT", newText }
            };

            dbService.ExecuteDML(sql, param);
        }

        public static void DeleteSimpleCommand(DatabaseService dbService, string name)
        {
            //TODO: rework permission and guild system

            //TODO: work in progress
        }

        public static bool IsCommandAvailable(DatabaseService dbService, string name)
        {
            string sql = "select 1 from simple_command where name = @NAME";

            var param = new Dictionary<string, object>()
            {
                { "@NAME", name}
            };

            var result = dbService.ExecuteScalar(sql, param) as int?;

            if(result == null)
                return false;

            return true;
        }

        #endregion

        #region Private Methods

        private static ulong? GetCommandAuthor(DatabaseService dbService, string name)
        {
            string sql = "select creator_id from simple_command where name = @NAME";

            var param = new Dictionary<string, object>()
            {
                { "@NAME", name}
            };

            var result = dbService.ExecuteScalar(sql, param) as ulong?;

            return result;
        }

        #endregion

        #endregion


    }
}