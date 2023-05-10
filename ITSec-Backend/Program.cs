using ITSec_Backend.Controllers;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text;

namespace ITSec_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";

            // Connect to database and initialize it.
            DatabaseBuilder databaseBuilder = new DatabaseBuilder(connectionString);
            databaseBuilder.BuildDatabase(true);

            // Test controller connection to Microsoft SQL 2022 Server Database.
            DatabaseController databaseController = new DatabaseController(connectionString);
            databaseController.ConnectToDatabase();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}