﻿using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;
using MySqlConnector;

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
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(this._connectionString);

                // Connect to SQL
                Console.WriteLine("Database builder: Connecting to SQL Server database ... ");

                using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
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

        private void CreateDatabase(MySqlConnection connection)
        {
            // Create a sample database
            Console.Write("Dropping and creating database 'SmartHomeDB' ... ");
            String sql = "DROP DATABASE IF EXISTS SmartHomeDB; CREATE DATABASE SmartHomeDB; USE SmartHomeDB";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Done.");
            }
        }

        private void CreateUserTable(MySqlConnection connection)
        {
            Console.Write("Creating table 'Users' ... ");

            StringBuilder sb = new StringBuilder();


            sb.Append("DROP TABLE IF EXISTS Users; ");
            sb.Append("CREATE TABLE Users ( ");
            sb.Append(" Username NVARCHAR(50) NOT NULL PRIMARY KEY, ");
            sb.Append(" Password NVARCHAR(50) NOT NULL ");
            sb.Append("); ");
            string sql = sb.ToString();

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Done.");
        }

        private void InsertUsers(MySqlConnection connection)
        {
            string sql = "INSERT INTO Users (Username, Password) VALUES (@username, @password);";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", "clemens123");
                command.Parameters.AddWithValue("@password", "hafenscher");
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) inserted into Users table.");
            }

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", "onur123");
                command.Parameters.AddWithValue("@password", "mete");
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) inserted into Users table.");
            }
        }

        private void CreateTemperatureTable(MySqlConnection connection) 
        {

        }

        private void InsertTemperature(MySqlConnection connection)
        {

        }

        private void CreateAdminTable(MySqlConnection connection)
        {
            Console.Write("Creating table 'Administrators' ... ");

            StringBuilder sb = new StringBuilder();
            
            sb.Append("DROP TABLE IF EXISTS Administrators; ");
            sb.Append("CREATE TABLE Administrators ( ");
            sb.Append(" Username NVARCHAR(50) NOT NULL PRIMARY KEY, ");
            sb.Append(" Password NVARCHAR(50) NOT NULL ");
            sb.Append("); ");
            string sql = sb.ToString();
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Done.");
        }

        private void InsertAdmin(MySqlConnection connection)
        {
            string sql = "INSERT INTO Administrators (Username, Password) VALUES (@username, @password);";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", "admin123");
                command.Parameters.AddWithValue("@password", "admin");
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) inserted into Administrators table.");
            }
        }
    }
}
