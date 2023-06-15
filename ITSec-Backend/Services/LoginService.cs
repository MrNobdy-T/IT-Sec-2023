using MySqlConnector;
using System.Net;

namespace ITSec_Backend.Services;

public class LoginService
{
    public HttpStatusCode CheckLogin(MySqlConnection connection, string username, string password)
    {
        try
        {
            // Open the connection
            connection.Open();

            IEnumerable<string> data = ReadDatabase(connection);

            // Create a SQL command to check the login credentials
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
            MySqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            Int64 count = (Int64)command?.ExecuteScalar()!;

            if (count > 0)
            {
                // Login successful
                return HttpStatusCode.OK;
            }
        }
        catch
        {
            // Handle any exceptions that occurred during the database operation
            return HttpStatusCode.Unauthorized;
        }

        return HttpStatusCode.Unauthorized;
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
}
