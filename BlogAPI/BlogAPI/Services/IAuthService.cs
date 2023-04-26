using BlogAPI.ViewModels;

namespace BlogAPI.Services;

public interface IAuthService
{
    public AuthData GetAuthData(string id);
    public string HashPassword(string password);
    public bool VerifyPassword(string actualPassword, string hashedPassword);
}
