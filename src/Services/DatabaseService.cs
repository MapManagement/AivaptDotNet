using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace AivaptDotNet.Services
{
    public class DatabaseService
    {
        private  MySqlConnection _connection;

        public DatabaseService(MySqlConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        public void Dispose()
        {
            _connection.Close();
        }

        public MySqlDataReader ExecuteSelect(string commandString, Dictionary<string, object> commandParameters)
        {
            MySqlCommand command = new MySqlCommand(commandString, _connection);

            foreach(var parameter in commandParameters)
            {
                MySqlDbType dbType = GetDbDataType(parameter.Value);
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            command.Prepare();
            var data = command.ExecuteReader();
            return data;
        }

        public void ExecuteDML(string commandString, Dictionary<string, object> commandParameters)
        {
            MySqlCommand command = new MySqlCommand(commandString, _connection);

            foreach(var parameter in commandParameters)
            {
                MySqlDbType dbType = GetDbDataType(parameter.Value);
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            command.Prepare();
            command.ExecuteNonQuery();
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
    }
}