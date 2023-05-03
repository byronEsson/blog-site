using BlogAPI.Data.Repositories;
using BlogAPI.Models;
using BlogAPI.Services;
using BlogAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    IAuthService _authService;
    IBlogService<User, string> _blogService;
    IBaseRepository<IdentityUserRole<string>, string> _userRoleRepository;
    IBaseRepository<IdentityRole, string> _roleRepository;
    private readonly ILogger _logger;


    public AuthController(IAuthService authService, IBlogService<User, string> blogService, IBaseRepository<IdentityUserRole<string>, string> repository, ILogger<AuthController> logger, IBaseRepository<IdentityRole, string> roleRepository)
    {
        _blogService = blogService;
        _authService = authService;
        _userRoleRepository = repository;
        _logger = logger;
        _roleRepository = roleRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthData>> Post([FromBody] LoginViewModel model)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var serviceResponse = _blogService.FindSingle(u => u.Email == model.Email);



        if (serviceResponse.Data == null) return BadRequest(new { email = serviceResponse.Message });

        var passwordValid = _authService.VerifyPassword(model.Password, serviceResponse.Data.PasswordHash);

        if(!passwordValid)
        {
            return BadRequest(new { password = "invalid password" });
        }
        //_logger.LogWarning(serviceResponse.Data.Roles[0].Name);

        var roles = (await _userRoleRepository.FindWhere(r => r.UserId == serviceResponse.Data.Id)).ToList();

        List<string> roleNames = new();

        foreach (var role in roles)
        {
            roleNames.Add((await _roleRepository.FindAsync(role.RoleId)).Name);
        }

        return _authService.GetAuthData(serviceResponse.Data, roleNames);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthData>> Post([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var emailUniq = IsEmailUniq(model.Email);
        if (!emailUniq) return BadRequest(new { email = "user with this email already exists" });
        var usernameUniq = IsUsernameUniq(model.UserName);
        if (!usernameUniq) return BadRequest(new { username = "user with this email already exists" });

        var id = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = id,
            UserName = model.UserName,
            Email = model.Email,
            PasswordHash = _authService.HashPassword(model.Password)
        };
        await _blogService.CreateAsync(user);
        await _blogService.SaveAsync();
        return _authService.GetAuthData(user, new List<string> { "Visitor"});
    }
    private bool IsEmailUniq(string email)
    {
        var user = _blogService.FindSingle(u => u.Email == email);
        return user != null;
    }
    private bool IsUsernameUniq(string username)
    {
        var user = _blogService.FindSingle(u => u.UserName == username);
        return user != null;
    }
}
