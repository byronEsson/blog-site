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

        if (context.Users.Any())
        {
            context.Users.RemoveRange(context.Users);
            context.Comments.RemoveRange(context.Comments);
            context.BlogPosts.RemoveRange(context.BlogPosts);
        }

        context.Users.Add(user);

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
            Author = user,
            BlogPost = post,
        };
        context.Comments.Add(comment);
        context.SaveChanges();
    }
}
