using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuickFinance.Api.Data;
using QuickFinance.Api.Models;
using System.Text.Json.Serialization;

namespace QuickFinance.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create a builder for the web application
            var builder = WebApplication.CreateBuilder(args);

            // Configure Database Contexts
            builder.Services.AddDbContext<FinanceContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity Services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<FinanceContext>()
                .AddDefaultTokenProviders();

            // Register TokenService as a scoped service
            builder.Services.AddScoped<TokenService>();

            // Add Controllers with JSON Configurations
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            // Add Swagger for API Documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", corsBuilder =>
                    corsBuilder.WithOrigins("http://localhost:8080")
                               .AllowAnyMethod()
                               .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/error"); // Production error handler
            }

            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Seed Database
            try
            {
                using (var serviceScope = app.Services.CreateScope())
                {
                    var serviceProvider = serviceScope.ServiceProvider;
                    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    await SeedDataUsers.Initialize(serviceProvider);
                    await SeedData.Initialize(serviceProvider, userManager);
                }
            }
            catch (Exception ex) { }


            await app.RunAsync();
        }
    }
}
