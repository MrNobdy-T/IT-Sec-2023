using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;

namespace ITSec_Backend
{
    public class DatabaseBuilder
    {
        private readonly string _connectionString;

        public DatabaseBuilder(string connectionString) 
        {
            this._connectionString = connectionString;
        }

        public void BuildDatabase(bool defaultTablesAndInserts)
        {
            try
            {
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(this._connectionString);

                // Connect to SQL
                Console.WriteLine("Database builder: Connecting to SQL Server database ... ");

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    Console.WriteLine("Database builder: Successfully connected!");

                    this.CreateDatabase(connection);

                    if (defaultTablesAndInserts) 
                    {
                        this.CreateUserTable(connection);
                        this.CreateAdminTable(connection);

                        this.InsertUsers(connection);
                        this.InsertAdmin(connection);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("All done. Press any key to finish...");
            Console.ReadKey(true);
        }

        private void CreateDatabase(SqlConnection connection)
        {
            // Create a sample database
            Console.Write("Dropping and creating database 'SmartHomeDB' ... ");
            String sql = "DROP DATABASE IF EXISTS [SmartHomeDB]; CREATE DATABASE [SmartHomeDB]";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Done.");
            }
        }

        private void CreateUserTable(SqlConnection connection)
        {
            Console.Write("Creating table 'Users' ... ");

            StringBuilder sb = new StringBuilder();

            sb.Append("DROP TABLE Users; ");
            sb.Append("CREATE TABLE Users ( ");
            sb.Append(" Username NVARCHAR(50) NOT NULL PRIMARY KEY, ");
            sb.Append(" Password NVARCHAR(50) NOT NULL ");
            sb.Append("); ");
            string sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Done.");
        }

        private void InsertUsers(SqlConnection connection)
        {
            string sql = "INSERT INTO Users (Username, Password) VALUES (@username, @password);";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", "clemens123");
                command.Parameters.AddWithValue("@password", "hafenscher");
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) inserted into Users table.");
            }

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", "onur123");
                command.Parameters.AddWithValue("@password", "mete");
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) inserted into Users table.");
            }
        }

        private void CreateTemperatureTable(SqlConnection connection) 
        {

        }

        private void InsertTemperature(SqlConnection connection)
        {

        }

        private void CreateAdminTable(SqlConnection connection)
        {
            Console.Write("Creating table 'Administrators' ... ");

            StringBuilder sb = new StringBuilder();

            sb.Append("DROP TABLE Administrators; ");
            sb.Append("CREATE TABLE Administrators ( ");
            sb.Append(" Username NVARCHAR(50) NOT NULL PRIMARY KEY, ");
            sb.Append(" Password NVARCHAR(50) NOT NULL ");
            sb.Append("); ");
            string sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Done.");
        }

        private void InsertAdmin(SqlConnection connection)
        {
            string sql = "INSERT INTO Administrators (Username, Password) VALUES (@username, @password);";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", "admin123");
                command.Parameters.AddWithValue("@password", "admin");
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) inserted into Administrators table.");
            }
        }
    }
}
