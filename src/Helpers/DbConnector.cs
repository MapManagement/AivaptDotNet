using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace AivaptDotNet.Helpers
{
    //TODO: reconfigure connections
    public class DbConnector
    {
        MySqlConnection Connection;
        public DbConnector(MySqlConnection connection)
        {
            Connection = connection;
            Connection.Open();
        }

        public MySqlDataReader ExecuteSelect(string commandString, Dictionary<string, object> commandParameters)
        {
            MySqlCommand command = new MySqlCommand(commandString, Connection);

            foreach(var parameter in commandParameters)
            {
                MySqlDbType dbType = GetDbDataType(parameter.Value);
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            command.Prepare();
            var data = command.ExecuteReader();
            return data;
        }

        private MySqlDbType GetDbDataType(object value)
        {
            switch(Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.String:
                    return MySqlDbType.VarChar;
                case TypeCode.Decimal:
                    return MySqlDbType.Decimal;
                case TypeCode.DateTime:
                    return MySqlDbType.DateTime;
                default:
                    return MySqlDbType.VarChar;
            }
        }

        public void ExecuteDML(string commandString, Dictionary<string, object> commandParameters)
        {
            MySqlCommand command = new MySqlCommand(commandString, Connection);

            foreach(var parameter in commandParameters)
            {
                MySqlDbType dbType = GetDbDataType(parameter.Value);
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            command.Prepare();
            command.ExecuteNonQuery();
        }
    }

}