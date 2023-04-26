using BlogAPI.Models;
using BlogAPI.ViewModels;

namespace BlogAPI.Services;

public interface IAuthService
{
    public AuthData GetAuthData(User user);
    public string HashPassword(string password);
    public bool VerifyPassword(string actualPassword, string hashedPassword);
}
