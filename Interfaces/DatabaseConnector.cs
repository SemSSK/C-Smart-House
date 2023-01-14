using MySql.Data.MySqlClient;

namespace Interfaces
{
    public class DatabaseConnector
    {
        private string connectionString = "Server=localhost;Database=SmartHome;Uid=root;Pwd=11111111;";

        public MySqlConnection GetConnetion() =>
            new MySqlConnection(connectionString);
        
    }
}