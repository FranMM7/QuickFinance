using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Email), IsUnique = true)] // Ensures Email is unique
public class ApplicationUser : IdentityUser
{
    //[Required] // Mark Username as required
    //[StringLength(100)] // Set maximum length
    //public new string UserName { get; set; } // Override the inherited UserName property

    //[Required] // Mark Email as required
    //[EmailAddress] // Ensures it is a valid email format
    //public new string Email { get; set; } // Override the inherited Email property

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
