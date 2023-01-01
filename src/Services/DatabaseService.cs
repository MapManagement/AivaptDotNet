using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;


namespace AivaptDotNet.Services
{
    public class DatabaseService
    {
        #region Fields

        private MySqlConnection _connection;

        #endregion

        #region Methods

        #region Public

        public void Dispose()
        {
            _connection.Close();
        }

        public async Task Initialize(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
            await _connection.OpenAsync();
        }

        public MySqlDataReader ExecuteSelect(string commandString, Dictionary<string, object> commandParameters = null)
        {
            MySqlCommand command = new MySqlCommand(commandString, _connection);

            if (commandParameters != null)
            {
                foreach (var parameter in commandParameters)
                {
                    MySqlDbType dbType = GetDbDataType(parameter.Value);
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                command.Prepare();
            }

            var data = command.ExecuteReader();
            return data;
        }

        public object ExecuteScalar(string commandString, Dictionary<string, object> commandParameters = null)
        {
            MySqlCommand command = new MySqlCommand(commandString, _connection);

            if (commandParameters != null)
            {
                foreach (var parameter in commandParameters)
                {
                    MySqlDbType dbType = GetDbDataType(parameter.Value);
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                command.Prepare();
            }

            var data = command.ExecuteScalar();
            return data;
        }

        public void ExecuteDML(string commandString, Dictionary<string, object> commandParameters = null)
        {
            MySqlCommand command = new MySqlCommand(commandString, _connection);

            if (commandParameters != null)
            {
                foreach (var parameter in commandParameters)
                {
                    MySqlDbType dbType = GetDbDataType(parameter.Value);
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                command.Prepare();
            }

            command.ExecuteNonQuery();
        }

        #endregion

        #region Private Methods

        private MySqlDbType GetDbDataType(object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.String:
                    return MySqlDbType.VarChar;
                case TypeCode.Decimal:
                    return MySqlDbType.Decimal;
                case TypeCode.Int32:
                    return MySqlDbType.Decimal;
                case TypeCode.DateTime:
                    return MySqlDbType.DateTime;
                default:
                    return MySqlDbType.VarChar;
            }
        }

        #endregion

        #endregion
    }
}
