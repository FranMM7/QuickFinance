using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)] // Adjust according to your password policy
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Required]
    public bool anynimousData { get; set; }

    public string? name { get; set; } = null;
    public string? middleName { get; set; } = null;
    public string? lastName { get; set; } = null;
}
