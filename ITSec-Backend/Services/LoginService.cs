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

            // Create a SQL command to check the login credentials
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
            MySqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            Int64 count = (Int64)command?.ExecuteScalar()!;

            if (count > 0)
                return HttpStatusCode.OK;
        }
        catch
        {
            // Handle any exceptions that occurred during the database operation
            return HttpStatusCode.Unauthorized;
        }

        return HttpStatusCode.Unauthorized;
    }
}
