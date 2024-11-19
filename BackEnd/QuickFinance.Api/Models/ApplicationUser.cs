using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Email), IsUnique = true)] // Ensures Email is unique
public class ApplicationUser : IdentityUser
{
    [DefaultValue(true)]
    public bool AnonymousData { get; set; } 

    [StringLength(150)]
    public string? Name { get; set; }

    [StringLength(150)]
    public string? MiddleName { get; set; }

    [StringLength(150)]
    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
