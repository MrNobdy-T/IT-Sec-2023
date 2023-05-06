using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;

namespace ITSec_Backend.Controllers
{
   // [ApiController]
   // [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly string _connectionString;

        public DatabaseController(string connectionString)
        {
            _connectionString = connectionString;
        }

       // [HttpGet(Name = "GetDatabase")]
        public IEnumerable<WeatherForecast> Get()
        {
            return default!;
        }

        public void ConnectToDatabase()
        {
            try
            {
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(this._connectionString);

                // Connect to SQL
                Console.WriteLine("Connecting to SQL Server database ... ");

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Successfully connected!");

                    this.CreateTestData(connection);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("All done. Press any key to finish...");
            Console.ReadKey(true);
        }

        private void CreateTestData(SqlConnection connection)
        {
            // Create a sample database
            Console.Write("Dropping and creating database 'SampleDB' ... ");
            String sql = "DROP DATABASE IF EXISTS [SampleDB]; CREATE DATABASE [SampleDB]";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Done.");
            }

            // Create a Table and insert some sample data
            Console.Write("Creating sample table with data, press any key to continue...");
            Console.ReadKey(true);
            StringBuilder sb = new StringBuilder();
            sb.Append("USE SampleDB; ");
            sb.Append("CREATE TABLE Employees ( ");
            sb.Append(" Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
            sb.Append(" Name NVARCHAR(50), ");
            sb.Append(" Location NVARCHAR(50) ");
            sb.Append("); ");
            sb.Append("INSERT INTO Employees (Name, Location) VALUES ");
            sb.Append("(N'Jared', N'Australia'), ");
            sb.Append("(N'Nikita', N'India'), ");
            sb.Append("(N'Tom', N'Germany'); ");
            sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Done.");
            }

            // INSERT demo
            Console.Write("Inserting a new row into table, press any key to continue...");
            Console.ReadKey(true);
            sb.Clear();
            sb.Append("INSERT Employees (Name, Location) ");
            sb.Append("VALUES (@name, @location);");
            sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@name", "Jake");
                command.Parameters.AddWithValue("@location", "United States");
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) inserted");
            }

            // UPDATE demo
            String userToUpdate = "Nikita";
            Console.Write("Updating 'Location' for user '" + userToUpdate + "', press any key to continue...");
            Console.ReadKey(true);
            sb.Clear();
            sb.Append("UPDATE Employees SET Location = N'United States' WHERE Name = @name");
            sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@name", userToUpdate);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) updated");
            }

            // DELETE demo
            String userToDelete = "Jared";
            Console.Write("Deleting user '" + userToDelete + "', press any key to continue...");
            Console.ReadKey(true);
            sb.Clear();
            sb.Append("DELETE FROM Employees WHERE Name = @name;");
            sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@name", userToDelete);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) deleted");
            }

            // READ demo
            Console.WriteLine("Reading data from table, press any key to continue...");
            Console.ReadKey(true);
            sql = "SELECT Id, Name, Location FROM Employees;";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                    }
                }
            }
        }
    }
}
