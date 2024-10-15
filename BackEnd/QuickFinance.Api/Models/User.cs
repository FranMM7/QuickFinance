using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // Add this for the [Index] attribute

[Index(nameof(Username), IsUnique = true)] // Ensures Username is unique
[Index(nameof(Email), IsUnique = true)] // Ensures Email is unique
public class User
{
    [Key] // Specifies that this is the primary key
    public Guid Id { get; set; } = Guid.NewGuid(); // Use Guid for unique identifier

    [Required] // Mark Username as required
    [StringLength(100)] // Set maximum length
    public string Username { get; set; }

    [Required] // Mark Email as required
    [EmailAddress] // Ensures it is a valid email format
    public string Email { get; set; }

    [Required] // Mark Password as required
    [StringLength(100, MinimumLength = 6)] // Set password length
    public string Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
}
