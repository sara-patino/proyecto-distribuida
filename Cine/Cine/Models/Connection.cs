using MySql.Data.MySqlClient;
using System;

namespace Cine.Models
{
    public class Connection
    {
        private string connectionString = "server=localhost; database=cine; user=root; password=";

        public MySqlConnection Connect()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
