using System.Data.SqlClient;


namespace AivaptDotNet.Helpers
{
    class DbConnector
    {
        SqlConnection Connection;
        public DbConnector(SqlConnection connection)
        {
            Connection = connection;
        }

        public object ExecuteSelect(string commandString)
        {
            SqlCommand command = new SqlCommand(commandString, Connection);
            command.Connection.Open();
            var data = command.ExecuteReader();
            return data;
        }

        public void ExecuteDML(string commandString)
        {
            SqlCommand command = new SqlCommand(commandString, Connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
        }
    }

}