using ArticleContentApi.Models;


public interface IReportRepository
{
    Task<IReadOnlyList<AuthorArticleReport>> GetAuthorsWithArticlesAsync();

    Task<IReadOnlyList<RecentArticleReport>> GetRecentArticlesAsync(string language, DateTime asOfUtc);
}



