using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuickFinance.Api;
using System;
using System.IO;
using System.Threading.Tasks;

public static class SeedDataUsers
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>(); // Inject logger

        // Define log file path for errors
        var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "user_seed_errors.log");
        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

        // Seed admin user
        await SeedUserAsync(userManager, roleManager, "admin", "admin@example.com", "Admin@123", "Admin", logFilePath, logger);

        // Seed regular user
        await SeedUserAsync(userManager, roleManager, "user", "user@example.com", "User@123", "User", logFilePath, logger);
    }

    private static async Task SeedUserAsync(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    string userName,
    string email,
    string password,
    string roleName,
    string logFilePath,
    ILogger logger)
    {
        // Debug log for tracking
        logger.LogInformation($"Seeding user: {userName} with email: {email}");

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = userName, // Explicitly setting UserName
                Email = email
            };

            // Confirm UserName and Email before creation
            logger.LogInformation($"Creating user with UserName: {user.UserName}, Email: {user.Email}");

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await userManager.AddToRoleAsync(user, roleName);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    var errorMessage = $"Error creating user {userName}: {error.Code}/{error.Description}";
                    logger.LogError(errorMessage);
                    await File.AppendAllTextAsync(logFilePath, $"Log:{logger.ToString()}\n");
                    await File.AppendAllTextAsync(logFilePath, $"{DateTime.UtcNow}: {errorMessage}\n");
                }
            }
        }
    }

}
