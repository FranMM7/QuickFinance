using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using System.Text.Json.Serialization;

namespace QuickFinance.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a builder for the web application
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            // Configure the database context to use SQL Server
            builder.Services.AddDbContext<FinanceContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure JSON options to handle circular references
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Enable reference handling
                });

            // Add support for exploring endpoints (needed for Swagger)
            builder.Services.AddEndpointsApiExplorer();

            // Add Swagger generation services for API documentation
            builder.Services.AddSwaggerGen();

            // Configure CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                    builder.WithOrigins("http://localhost:8080") // Allow your frontend URL
                           .AllowAnyMethod() // Allow any HTTP method
                           .AllowAnyHeader()); // Allow any headers
            });

            // Build the application
            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); // Enable Swagger for API documentation
                app.UseSwaggerUI(); // Enable Swagger UI for interactive documentation
            }

            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

            app.UseCors("AllowSpecificOrigin"); // Apply the defined CORS policy

            app.UseAuthorization(); // Enable authorization features
            app.MapControllers(); // Map controller routes to the application

            // Seed the database with initial data
            using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<FinanceContext>(); // Get the FinanceContext
                SeedData.Initialize(serviceScope.ServiceProvider); // Call the SeedData method to initialize the database
            }

            app.Run(); // Start the application and listen for incoming HTTP requests
        }
    }
}
