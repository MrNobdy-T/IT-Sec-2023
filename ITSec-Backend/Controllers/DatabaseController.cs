using System.Data.SqlClient;
using System.Net;
using ITSec_Backend.Data;
using ITSec_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace ITSec_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly string _connectionString = "server=127.0.0.1;uid=root;pwd=root;database=SmartHomeDB";

    private readonly LoginService _loginService = new();

    [ApiExplorerSettings(IgnoreApi = true)]
    public void ConnectToDatabase()
    {
        try
        {
            // Build connection string
            MySqlConnectionStringBuilder builder = new(_connectionString);
            // Connect to SQL
            Console.WriteLine("Database controller: Connecting to SQL Server database ... ");

            using MySqlConnection connection = new(builder.ConnectionString);
            connection.Open();

            Console.WriteLine("Database controller: Successfully connected!");
            ReadDatabase(connection);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("All done. Press any key to finish...");
    }

    private static IEnumerable<string> ReadDatabase(MySqlConnection connection)
    {
        List<string> databaseContent = new();

        Console.WriteLine("Reading database content ...");

        // Read users
        Console.WriteLine("Users:");
        using (MySqlCommand command = new("SELECT * FROM Users", connection))
        {
            using MySqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                databaseContent.Add($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}, Role: {reader.GetString(2)}");
                Console.WriteLine($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}, Role: {reader.GetString(2)}");
            }
        }

        // Read temperature
        Console.WriteLine("Temperature:");
        using (MySqlCommand command = new("SELECT * FROM Temperature", connection))
        {
            using MySqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                double temperatureValue = Math.Round(reader.GetDouble(0), 2);
                DateTime timestamp = reader.GetDateTime(1);
                databaseContent.Add($"Value: {temperatureValue}, Time: {timestamp}");
                Console.WriteLine($"Value: {temperatureValue}, Time: {timestamp}");
            }
        }

        Console.WriteLine("Done.");
        return databaseContent;
    }

    [HttpGet("database", Name = "GetDatabase")]
    public IEnumerable<string> Get()
    {
        // Build connection string
        MySqlConnectionStringBuilder builder = new(_connectionString);

        using MySqlConnection connection = new(builder.ConnectionString);
        connection.Open();
        return ReadDatabase(connection);
    }

    [HttpGet("temperature", Name = "GetTemperature")]
    public IEnumerable<string> GetTemperature()
    {
        List<string> databaseContent = new();

        // Retrieve data from the "Temperature" table
        using (MySqlConnection connection = new(_connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM Temperature;";
            using MySqlCommand command = new(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                double temperatureValue = Math.Round(reader.GetDouble(0), 2);
                DateTime timestamp = reader.GetDateTime(1);
                string entry = $"Value: {temperatureValue}, Time: {timestamp}";
                databaseContent.Add(entry);
            }
        }

        return databaseContent;
    }

    [HttpPost("login", Name = "PostLogin")]
    public IActionResult PostLogin([FromBody] LoginRequest loginRequest)
    {
        // Build connection string
        MySqlConnectionStringBuilder builder = new(_connectionString);
        HttpStatusCode code;

        using (MySqlConnection connection = new(builder.ConnectionString))
        {
            code = _loginService.CheckLogin(connection, loginRequest.Username, loginRequest.Password);
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
        MySqlConnectionStringBuilder builder = new(this._connectionString);

        using (MySqlConnection connection = new(builder.ConnectionString))
        {
            connection.Open();

            using MySqlCommand command = new("SELECT Role FROM Users WHERE Username = @username AND Password = @password", connection);
            command.Parameters.AddWithValue("@username", loginRequest.Username);
            command.Parameters.AddWithValue("@password", loginRequest.Password);

            using MySqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                role = reader.GetString(0);
            }
        }

        return role;
    }
}
