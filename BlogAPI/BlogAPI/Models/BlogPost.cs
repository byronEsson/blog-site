namespace BlogAPI.Models;

public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Body { get; set; } = string.Empty!;

    public List<Comment>? Comments { get; set; } = new List<Comment>();
}
