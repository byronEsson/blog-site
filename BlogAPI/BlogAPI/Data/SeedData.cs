using BlogAPI.Models;
using CryptoHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;

namespace BlogAPI.Data;

public class SeedData
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<BlogContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleStore = new RoleStore<IdentityRole>(context);

        var user = new User
        {
            UserName = "byron",
            Email = "byronesson767@gmail.com",
            EmailConfirmed = false,
            PasswordHash = Crypto.HashPassword("Password1!")
        };

        var userTwo = new User
        {
            UserName = "stew",
            Email = "stewie@gmail.com",
            EmailConfirmed = false,
            PasswordHash = Crypto.HashPassword("Password1!")
        };

        if (context.Roles.Any())
        {
            context.Users.RemoveRange(context.Users);
            context.Comments.RemoveRange(context.Comments);
            context.BlogPosts.RemoveRange(context.BlogPosts);
            context.Roles.RemoveRange(context.Roles);
            context.UserRoles.RemoveRange(context.UserRoles);
            context.SaveChanges();
        }

        var admin = new IdentityRole
        {
            Name = "Admin",
            NormalizedName = "admin",
        };

        var visitor = new IdentityRole
        {
            Name = "Visitor",
            NormalizedName = "visitor"
        };

        roleStore.CreateAsync(admin)
            .GetAwaiter().GetResult();
        roleStore.CreateAsync(visitor)
            .GetAwaiter().GetResult();
        context.Users.AddRange(user, userTwo);

        context.UserRoles.Add(
            new IdentityUserRole<string>
            {
                UserId = userManager.GetUserIdAsync(user).Result,
                RoleId = roleStore.GetRoleIdAsync(admin).Result
            }
        );

        var post = new BlogPost
        {
            Title = "Lorem Ipsum",
            CreatedAt = new DateTime(2023, 01, 31),
            Body = Lorem.LongText
        };
        context.BlogPosts.Add(post);

        var comment = new Comment
        {
            Body = Lorem.ShortText,
            CreatedAt = new DateTime(2023, 02, 01),
            Author = userTwo,
            BlogPost = post,
        };
        context.Comments.Add(comment);
        context.SaveChanges();
    }
}
