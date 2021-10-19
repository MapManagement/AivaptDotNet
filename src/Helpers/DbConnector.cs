using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace AivaptDotNet.Helpers
{
    class DbConnector
    {
        SqlConnection Connection;
        public DbConnector(SqlConnection connection)
        {
            Connection = connection;
        }

        public SqlDataReader ExecuteSelect(string commandString, Dictionary<string, object> commandParameters)
        {
            SqlCommand command = new SqlCommand(commandString, Connection);

            foreach(var parameter in commandParameters)
            {
                SqlDbType dbType = GetDbDataType(parameter.Value);
                command.Parameters.Add(parameter.Key, dbType);
            }

            command.Connection.Open();
            var data = command.ExecuteReader();
            return data;
        }

        private SqlDbType GetDbDataType(object value)
        {
            switch(Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.String:
                    return SqlDbType.VarChar;
                case TypeCode.Decimal:
                    return SqlDbType.Decimal;
                case TypeCode.DateTime:
                    return SqlDbType.DateTime;
                default:
                    return SqlDbType.VarChar;
            }
        }

        public void ExecuteDML(string commandString)
        {
            SqlCommand command = new SqlCommand(commandString, Connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
        }
    }

}