using System.Collections.Generic;
using System.Linq;
using AivaptDotNet.Services.Database;
using AivaptDotNet.Services.Database.Models;

namespace AivaptDotNet.Helpers.Modules
{
    public static class SimpleCommandHelper
    {
        #region Methods

        #region Public

        public static SimpleCommand GetSimpleCommand(BotDbContext dbContext, string commandName)
        {
            var simpleCommand = dbContext.SimpleCommands.Find(commandName);

            return simpleCommand;
        }

        public static List<SimpleCommand> GetAllSimpleCommands(BotDbContext dbContext)
        {
            return dbContext.SimpleCommands.ToList();
        }

        public static void CreateSimpleCommand(BotDbContext dbContext, string name, string text, ulong authorId)
        {
            var simpleCommand = new SimpleCommand 
            {
                Name = name,
                Text = text,
                CreatorId = authorId
            };

            dbContext.SimpleCommands.Add(simpleCommand);
            dbContext.SaveChanges();    
        }

        public static void EditSimpleCommand(BotDbContext dbContext, string name, string newText)
        {
            var simpleCommand = dbContext.SimpleCommands.Find(name);
            simpleCommand.Name = name;
            simpleCommand.Text = newText;

            dbContext.SaveChanges();
        }

        public static void DeleteSimpleCommand(BotDbContext dbContext, string name)
        {
            //TODO: rework permission and guild system

            //TODO: work in progress
        }

        public static bool IsCommandAvailable(BotDbContext dbContext, string name)
        {
            var simpleCommand = dbContext.SimpleCommands.Find(name);

            return simpleCommand != null;
        }

        #endregion

        #region Private

        private static ulong? GetCommandAuthor(BotDbContext dbContext, string name)
        {
            var simpleCommand = dbContext.SimpleCommands.Find(name);

            return simpleCommand?.CreatorId;
        }

        #endregion

        #endregion
    }
}
