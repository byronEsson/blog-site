using BlogAPI.Data.Repositories;
using BlogAPI.Models;
using BlogAPI.Services;
using BlogAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    IAuthService _authService;
    IUserRepository<User, string> _repository;

    public AuthController(IAuthService authService, IUserRepository<User, string> repository)
    {
        _authService = authService;
        _repository = repository;
    }

    [HttpPost("login")]
    public ActionResult<AuthData> Post([FromBody] LoginViewModel model)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var user = _repository.GetSingle(u => u.Email == model.Email);

        if (user == null) return BadRequest(new { email = "no user with this email" });

        var passwordValid = _authService.VerifyPassword(model.Password, user.PasswordHash);

        if(!passwordValid)
        {
            return BadRequest(new { password = "invalid password" });
        }
        return _authService.GetAuthData(user.Id);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthData>> Post([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var emailUniq = _repository.IsEmailUniq(model.Email);
        if (!emailUniq) return BadRequest(new { email = "user with this email already exists" });
        var usernameUniq = _repository.IsUsernameUniq(model.UserName);
        if (!usernameUniq) return BadRequest(new { username = "user with this email already exists" });

        var id = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = id,
            UserName = model.UserName,
            Email = model.Email,
            PasswordHash = _authService.HashPassword(model.Password)
        };
        _repository.Add(user);
        await _repository.SaveAsync();
        return _authService.GetAuthData(id);
    }
}
