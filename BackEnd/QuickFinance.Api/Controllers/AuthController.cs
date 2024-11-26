using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;

    public AuthController(UserManager<ApplicationUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        // Check if the model is valid (this is optional, but good for validation purposes)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Create the new user
        var user = new ApplicationUser { UserName = model.Username, Email = model.Email, AnonymousData=model.anynimousData, Name=model.name, MiddleName=model.middleName, LastName=model.lastName };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Assign the default role "User"
            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                return BadRequest("Failed to assign role.");
            }

            // Create the token for the newly registered user
            var token = _tokenService.CreateToken(user);

            // Get the roles of the user (to include in the response if needed)
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                token,
                userId = user.Id,
                userName = user.UserName,
                roles
            });
        }

        // If creation failed, return the errors
        return BadRequest(result.Errors);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("add-admin")]
    public async Task<IActionResult> AddAdmin([FromBody] string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound("User not found");
        }

        var isInRole = await _userManager.IsInRoleAsync(user, "Admin");
        if (!isInRole)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
            return Ok("User has been made an admin.");
        }

        return BadRequest("User is already an admin.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("change-role")]
    public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null)
        {
            return NotFound("User not found");
        }

        // Remove the user from their current role
        var currentRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in currentRoles)
        {
            await _userManager.RemoveFromRoleAsync(user, role);
        }

        // Add the user to the new role
        var result = await _userManager.AddToRoleAsync(user, model.NewRole);
        if (result.Succeeded)
        {
            return Ok("User's role has been changed.");
        }

        return BadRequest("Error changing role.");
    }




    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var token = _tokenService.CreateToken(user);

            // Retrieve the user's role
            var roles = await _userManager.GetRolesAsync(user);
            var fullname = user.AnonymousData ? "" : user.Name + " " + user.LastName;
            return Ok(new
            {
                token,
                userId = user.Id,
                UserName = user.UserName,
                fullName = fullname,
                anonymousData = user.AnonymousData,
                firstName=user.Name,
                middleName=user.MiddleName,
                lastName=user.LastName,
                roles
            });
        }

        //var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();


        return Unauthorized("Invalid credentials");
    }

    [Authorize] // Restrict access to authenticated users
    [HttpPut("update-user-info")]
    public async Task<IActionResult> UpdateUserInfo(UpdateUserInfoDto model)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized("Unable to identify the user.");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Update the fields the user is allowed to modify
        user.Email = model.Email ?? user.Email;
        user.Name = model.Name ?? user.Name;
        user.MiddleName = model.MiddleName ?? user.MiddleName;
        user.LastName = model.LastName ?? user.LastName;
        
        if (model.anonymousData != user.AnonymousData)
        {
            user.AnonymousData = user.AnonymousData;
        }

        // Save changes
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return Ok(new { message = "User information updated successfully." });
        }

        return BadRequest(result.Errors);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> getUserInfo()
    {
        // Get the authenticated user's ID from claims
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("NameIdentifier claim is missing or empty.");
            return Unauthorized("Unable to identify the user.");
        }

        // Retrieve the user from the database
        var user = await _userManager.Users
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.Name,
                u.MiddleName,
                u.LastName,
                u.AnonymousData
                // Add any other fields you want to include
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user);
    }



    [Authorize] // Restrict access to authenticated users
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized("Unable to identify the user.");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }




        // Attempt to change the password
        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (result.Succeeded)
        {
            return Ok(new { message = "Password changed successfully." });
        }

        return BadRequest(result.Errors);
    }

    [HttpGet("debug-claims")]
    public IActionResult DebugClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(claims);
    }



}
