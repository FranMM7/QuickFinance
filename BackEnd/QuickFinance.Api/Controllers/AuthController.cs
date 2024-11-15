using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Assign the "User" role by default
            await _userManager.AddToRoleAsync(user, "User");

            var token = _tokenService.CreateToken(user);
            return Ok(new { token });
        }

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

            return Ok(new
            {
                token,
                userId = user.Id,
                roles
            });
        }

        return Unauthorized("Invalid credentials");
    }

}
