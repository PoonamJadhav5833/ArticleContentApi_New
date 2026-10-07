namespace ArticleContentApi.Models;

public  class AuthorArticleReport
{
    public int Id { get; set; }

    public string Title { get; set; }

    public IReadOnlyList<int> ArticleIds { get; set; }
}
