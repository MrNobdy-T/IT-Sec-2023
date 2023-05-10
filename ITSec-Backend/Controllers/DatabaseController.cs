using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;

namespace ITSec_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly string _connectionString;

        public DatabaseController(string connectionString)
        {
            this._connectionString = connectionString;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void ConnectToDatabase()
        {
            try
            {
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(this._connectionString);

                // Connect to SQL
                Console.WriteLine("Database controller: Connecting to SQL Server database ... ");

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
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
            Console.ReadKey(true);
        }

        private IEnumerable<string> ReadDatabase(SqlConnection connection)
        {
            List<string> databaseContent = new List<string>();

            Console.WriteLine("Reading database content ...");

            // Read users
            Console.WriteLine("Users:");
            using (SqlCommand command = new SqlCommand("SELECT * FROM Users", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databaseContent.Add($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}");
                        Console.WriteLine($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}");
                    }
                }
            }

            // Read administrators
            Console.WriteLine("Administrators:");
            using (SqlCommand command = new SqlCommand("SELECT * FROM Administrators", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databaseContent.Add($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}");
                        Console.WriteLine($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}");
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
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(this._connectionString);
            
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                return this.ReadDatabase(connection);
            }
        }
    }
}
