using System;
using System.IO;

namespace AivaptDotNet.Helpers
{
    static class Credentials
    {
        public static string GetBotToken()
        {
            string token = File.ReadAllText("token.txt");
            return token;
        }

        public static string GetDbConnection()
        {
            string dbConnection = File.ReadAllText("sql_connection_string.txt");
            return dbConnection;
        }

        public static ulong GetAdminId()
        {
            ulong adminId = Convert.ToUInt64(File.ReadAllText("admin_id.txt"));
            return adminId;
        }
    }
}