using ArticleContentApi.Data;
using ArticleContentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleContentApi.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly AppDbContext _db;

    public ArticleRepository(AppDbContext db) => _db = db;

    public async Task<Article> AddAsync(CreateArticleRequest request)
    {
        var article = new Article
        {
            Status = request.Status,
            CreatedAt = DateTime.UtcNow
        };

        _db.Articles.Add(article);
        await _db.SaveChangesAsync();

        return article;
    }

    public async Task<bool> UpdateAsync(Article article)
    {
        var existing = await _db.Articles.FindAsync(article.Id);

        if (existing is null)
            return false;

        existing.Status = article.Status;
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _db.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var article = await _db.Articles.FindAsync(id);

        if (article is null)
            return false;

        _db.Articles.Remove(article);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<(IReadOnlyList<ArticleListItem> Items, int TotalCount)> GetArticles(
        GetArticlesRequest request)
    {
        var query = _db.Articles.AsNoTracking();

        if (request.Status.HasValue)
            query = query.Where(a => a.Status == request.Status.Value);

        var totalCount = await query.CountAsync();

        var articleList = query.Select(a => new ArticleListItem(
            a.Id,
            a.Contents
                .Where(c => c.Language == Language.English)
                .OrderBy(c => c.Id)
                .Select(c => c.Title)
                .FirstOrDefault()
            ?? a.Contents
                .OrderBy(c => c.Id)
                .Select(c => c.Title)
                .FirstOrDefault(),

            a.Contents
                .Where(c => c.Language == Language.English)
                .OrderBy(c => c.Id)
                .Select(c => c.Author!.Username)
                .FirstOrDefault()
            ?? a.Contents
                .OrderBy(c => c.Id)
                .Select(c => c.Author!.Username)
                .FirstOrDefault()
            ?? string.Empty,

            a.Status,
            a.CreatedAt));

        var asc = request.SortDirection.Equals(
            "asc",
            StringComparison.OrdinalIgnoreCase);

        articleList = request.SortBy.ToLowerInvariant() switch
        {
            "title" => asc
                ? articleList.OrderBy(x => x.Title).ThenBy(x => x.Id)
                : articleList.OrderByDescending(x => x.Title).ThenByDescending(x => x.Id),

            _ => asc
                ? articleList.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id)
                : articleList.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

        var items = await articleList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
