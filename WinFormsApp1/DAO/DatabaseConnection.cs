using MySql.Data.MySqlClient;

namespace WinFormsApp1.DAO
{
    public class DatabaseConnection
    {
        private const string server = "db4free.net"; //
        private const string port = "3306";
        private const string database = "gym_db";
        private const string username = "gurjant";
        private const string password = "Gurjant12";

        private static string connectionString = "Server=" + server + ";Port=" + port + ";Database=" + database + ";Uid=" + username + ";Pwd=" + password + ";OldGuids=true;";

        private static MySqlConnection? connection;

        public DatabaseConnection()
        {
            connection = new MySqlConnection(connectionString);
        }

        public static MySqlConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection(connectionString);
            }

            // Open the connection if it's not already open
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
