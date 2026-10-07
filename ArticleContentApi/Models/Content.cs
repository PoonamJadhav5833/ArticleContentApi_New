namespace ArticleContentApi.Models;

public class Content
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string ContentText { get; set; } = string.Empty;

    public int AuthorId { get; set; }

    public Status Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public Language Language { get; set; }

    public int ArticleId { get; set; }

    public Article Article { get; set; }

    public User Author { get; set; }
}
