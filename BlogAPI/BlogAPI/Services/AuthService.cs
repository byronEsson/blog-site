using BlogAPI.Data.Repositories;
using BlogAPI.Models;
using BlogAPI.ViewModels;
using CryptoHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPI.Services;

public class AuthService : IAuthService
{
    private string _jwtSecret;
    private int _jwtLifespan;


    public AuthService(string jwtSecret, int jwtLifespan)
    {
        _jwtSecret = jwtSecret;
        _jwtLifespan = jwtLifespan;
    }

    public AuthData GetAuthData(User user, List<string> roles)
    {
        var expirationTime = DateTime.UtcNow.AddSeconds(_jwtLifespan);



        List<Claim> additionalClaims = new();
        foreach (var role in roles)
        {
            additionalClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        additionalClaims.Add(new Claim(ClaimTypes.Name, user.Id));

        var identity = new ClaimsIdentity(additionalClaims.ToArray());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = expirationTime,
            // new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
                SecurityAlgorithms.HmacSha256Signature
            ),
           
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));


        return new AuthData
        {
            Token = token,
            TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds(),
            Id = user.Id,
            UserName = user.UserName
        };
    }

    public string HashPassword(string password)
    {
        return Crypto.HashPassword(password);
    }

    public bool VerifyPassword(string actualPassword, string hashedPassword)
    {
        return Crypto.VerifyHashedPassword(hashedPassword, actualPassword);
    }
}
