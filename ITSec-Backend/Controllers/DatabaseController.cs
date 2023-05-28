﻿using ITSec_Backend.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using MySqlConnector;

namespace ITSec_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly string _connectionString = "server=127.0.0.1;uid=root;pwd=root;database=smarthomedb";

        [ApiExplorerSettings(IgnoreApi = true)]
        public void ConnectToDatabase()
        {
            try
            {
                // Build connection string
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(this._connectionString);
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(_connectionString);
                // Connect to SQL
                Console.WriteLine("Database controller: Connecting to SQL Server database ... ");

                using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    Console.WriteLine("Database controller: Successfully connected!");
                    this.ReadDatabase(connection);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("All done. Press any key to finish...");
        }

        private IEnumerable<string> ReadDatabase(MySqlConnection connection)
        {
            List<string> databaseContent = new List<string>();

            Console.WriteLine("Reading database content ...");

            // Read users
            Console.WriteLine("Users:");
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM Users", connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databaseContent.Add($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}, Role: {reader.GetString(2)}");
                        Console.WriteLine($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}, Role: {reader.GetString(2)}");
                    }
                }
            }

            // Read temperature
            Console.WriteLine("Temperature:");
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM Temperature", connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double temperatureValue = Math.Round(reader.GetDouble(0), 2);
                        DateTime timestamp = reader.GetDateTime(1);
                        databaseContent.Add($"Value: {temperatureValue}, Time: {timestamp}");
                        Console.WriteLine($"Value: {temperatureValue}, Time: {timestamp}");
                    }
                }
            }

            Console.WriteLine("Done.");
            return databaseContent;
        }

        [HttpGet("database", Name = "GetDatabase")]
        public IEnumerable<string> Get()
        {
            // Build connection string
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(this._connectionString);

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                connection.Open();
                return this.ReadDatabase(connection);
            }
        }

        [HttpGet("temperature", Name = "GetTemperature")]
        public IEnumerable<string> GetTemperature()
        {
            List<string> databaseContent = new List<string>();

            // Retrieve data from the "Temperature" table
            using (MySqlConnection connection = new MySqlConnection(this._connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Temperature;";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double temperatureValue = Math.Round(reader.GetDouble(0), 2);
                            DateTime timestamp = reader.GetDateTime(1);
                            string entry = $"Value: {temperatureValue}, Time: {timestamp}";
                            databaseContent.Add(entry);
                        }
                    }
                }
            }

            return databaseContent;
        }

        [HttpPost("login", Name = "PostLogin")]
        public IActionResult PostLogin([FromBody] LoginRequest loginRequest)
        {
            // Build connection string
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(_connectionString);

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a SQL command to check the login credentials
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", loginRequest.Username);
                    command.Parameters.AddWithValue("@password", loginRequest.Password);

                    Int64 count = (Int64)command.ExecuteScalar();

                    if (count > 0)
                    {
                        // Login successful
                        // Create a success response with a custom message
                        Console.WriteLine("Success!");
                        var response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.ReasonPhrase = "Login successful";
                        return Ok(response);
                    }

                    Console.WriteLine("Failed");
                    // Invalid login credentials
                    // Handle the error or return an appropriate response
                }
                catch
                {
                    // Handle any exceptions that occurred during the database operation
                }
            }
            
            // Create a failure response with a custom message
            var errorResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            errorResponse.ReasonPhrase = "Login invalid";
            return Unauthorized(errorResponse);
        }
    }
}
