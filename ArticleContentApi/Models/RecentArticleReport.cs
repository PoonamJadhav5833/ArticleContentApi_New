namespace ArticleContentApi.Models;

public  class RecentArticleReport
{
    public int ArticleId { get; set; }

    public DateTime ArticleCreatedAt { get; set; }

    public string ArticleStatus { get; set; }

    public int ContentId { get; set; }

    public string Title { get; set; }

    public string Language { get; set; }

    public int AuthorId { get; set; }

    public string Author { get; set; }

    public DateTime AuthorCreatedAt { get; set; }

    public IReadOnlyList<int> ArticleIds { get; set; }
}

