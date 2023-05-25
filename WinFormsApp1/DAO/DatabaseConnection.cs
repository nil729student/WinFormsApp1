using MySql.Data.MySqlClient;

namespace WinFormsApp1.DAO
{   //classe que conté la connexió a la base de dades
    public class DatabaseConnection
    {   //connexió a la base de dades amb les dades de db4free.net
        private const string server = "db4free.net"; //
        private const string port = "3306";
        private const string database = "gym_db";
        private const string username = "gurjant";
        private const string password = "Gurjant12";

        //construir la connexió
        private static string connectionString = "Server=" + server + ";Port=" + port + ";Database=" + database + ";Uid=" + username + ";Pwd=" + password + ";OldGuids=true;";

        private static MySqlConnection? connection;

        //constructor de la classe
        public DatabaseConnection()
        {
            connection = new MySqlConnection(connectionString);
        }

        //mètode per obtenir la connexió
        public static MySqlConnection GetConnection()
        {
            // Crea la connexió si no existeix
            if (connection == null)
            {
                connection = new MySqlConnection(connectionString);
            }

            // Obre la connexió si no està oberta
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
