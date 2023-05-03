using BlogAPI.Models;
using BlogAPI.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Services;

public interface IAuthService
{
    public AuthData GetAuthData(User user, List<string> roles);
    public string HashPassword(string password);
    public bool VerifyPassword(string actualPassword, string hashedPassword);
}
