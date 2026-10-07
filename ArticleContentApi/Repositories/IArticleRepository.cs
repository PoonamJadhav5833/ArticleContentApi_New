using ArticleContentApi.Models;

public interface IArticleRepository
{
    Task<Article> AddAsync(CreateArticleRequest request);

    Task<bool> UpdateAsync(Article article);

    Task<Article?> GetByIdAsync(int id);

    Task<bool> DeleteAsync(int id);

    Task<(IReadOnlyList<ArticleListItem> Items, int TotalCount)> GetArticles(GetArticlesRequest request);
}
