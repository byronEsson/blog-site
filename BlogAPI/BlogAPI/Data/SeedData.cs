using BlogAPI.Models;
using CryptoHelper;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Data;

public class SeedData
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<BlogContext>();
        var user = new User
        {
            UserName = "byron",
            Email = "byronesson767@gmail.com",
            EmailConfirmed = false,
            PasswordHash = Crypto.HashPassword("Password1!")
        };

        if(context.Users.Any()) { context.Users.RemoveRange(context.Users); }

        context.Users.Add(user);
        context.SaveChanges();
    }
}
