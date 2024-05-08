using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly ILogger _logger;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

    [AllowAnonymous]
[HttpPost("login")]
public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
{
    var user = await _userManager.Users.Include(p => p.Photos)
        .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

    if (user == null)
        return Unauthorized();

    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

    if (result.Succeeded)
    {
        // Check if user is logging in with specific email
        if (user.Email.ToLower() == "darn@test.com")
        {
            // Check if user is not already an admin
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                // Assign Admin role to the user
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);

        // Add roles to UserDto
        var userDto = CreateUserObject(user);
        userDto.Roles = roles.ToArray();

        return userDto;
    }

    return Unauthorized();
}

        [AllowAnonymous]
[HttpPost("register")]
public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
{       
    // Other registration logic...

    var user = new AppUser
    {
        DisplayName = registerDto.DisplayName,
        Email = registerDto.Email,
        UserName = registerDto.Username
    };

    var result = await _userManager.CreateAsync(user, registerDto.Password);

    if (result.Succeeded)
    {
        // Assign role based on email
        var roles = new List<string>();
        if (registerDto.Email.ToLower() == "iziversanov@test.com")
        {
            roles.Add("Admin");
        }
        else
        {
            roles.Add("User");
        }

        // Add roles to UserDto
        var userDto = CreateUserObject(user);
        userDto.Roles = roles.ToArray();

        return userDto;
    }

        return BadRequest(result.Errors);
    }



    
        [Authorize]
        [HttpGet("user")]
        public ActionResult<IEnumerable<string>> UserPage()
        {
            return Ok("User page");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
                
            return CreateUserObject(user);
        }


     [Authorize(Policy = "AdminPolicy")]
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {


         var isAdmin = User.IsInRole("Admin");

    // Check if the user has the Admin role claim
    if (!isAdmin)
    {
        // Log the event
        _logger.LogWarning("Unauthorized access to /users endpoint by user {UserId}", User.Identity.Name);
        
        return Forbid(); // Return 403 Forbidden if the user doesn't have the Admin role
    }

    
        var users = await _userManager.Users.ToListAsync();
        var usersDto = users.Select(user => CreateUserObject(user)).ToList();
        return Ok(usersDto);
    }

        

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }
    }
}
