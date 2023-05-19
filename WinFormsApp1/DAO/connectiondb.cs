

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WinFormsApp1.DAO
{
    internal class connectiondb
    {

    }

    string connectionString = "server=db4free.net; user=gurjant;password=Gurjant12;";
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {

        string databaseName = "nombre_base_datos";
    string query = $"CREATE DATABASE {databaseName};";

    connection.Open();
        MySqlCommand command = new MySqlCommand(query, connection);
    command.ExecuteNonQuery();
    }
}


