using Microsoft.Identity.Client;

namespace BlogAPI.Models;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public string AuthorId { get; set; }
    public User Author { get; set; }
    public int BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }
}
