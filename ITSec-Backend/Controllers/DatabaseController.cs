using ITSec_Backend.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using MySqlConnector;
using MaliciousLoginPackage;

namespace ITSec_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly string _connectionString = "server=127.0.0.1;uid=root;pwd=root;database=smarthomedb";

        private readonly LoginManager _loginManager = new LoginManager();

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
            HttpStatusCode code;

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                code = this._loginManager.CheckLogin(connection, loginRequest.Username, loginRequest.Password);
            }

            var response = new HttpResponseMessage(code);

            switch (code)
            {
                case HttpStatusCode.Unauthorized:
                    response.ReasonPhrase = "Login invalid";
                    return Unauthorized(response);

                case HttpStatusCode.OK:
                    response.ReasonPhrase = "Login success";
                    return Ok(response);
            }

            return Unauthorized(response);
        }

        [HttpGet("role", Name = "GetRole")]
        public string GetRole([FromQuery] LoginRequest loginRequest)
        {
            string role = string.Empty;

            // Build connection string
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(this._connectionString);

            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("SELECT Role FROM Users WHERE Username = @username AND Password = @password", connection))
                {
                    command.Parameters.AddWithValue("@username", loginRequest.Username);
                    command.Parameters.AddWithValue("@password", loginRequest.Password);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            role = reader.GetString(0);
                        }
                    }
                }
            }

            return role;
        }
    }
}
