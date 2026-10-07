using ArticleContentApi.Data;
using ArticleContentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleContentApi.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _db;

    public ReportRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<AuthorArticleReport>> GetAuthorsWithArticlesAsync()
    {
        var users = await _db.Users.AsNoTracking()
            .Select(u => new
            {
                u.Id,
                u.Username,
                ArticleIds = u.Contents.Select(c => c.ArticleId)
                    .Distinct()
                    .OrderBy(id => id)
                    .ToList()
            })
            .OrderBy(x => x.Username)
            .ToListAsync();

        return users.Select(x =>
            new AuthorArticleReport(x.Id, x.Username, x.ArticleIds)).ToList();
    }

    public async Task<IReadOnlyList<RecentArticleReport>> GetRecentArticlesAsync(string language,DateTime asOfUtc)
    {
        if (!Enum.TryParse<Language>(language, true, out var languageEnum))
            return [];

        var articleFrom = asOfUtc.AddMonths(-3);
        var userFrom = asOfUtc.AddMonths(-4);

        return await _db.Contents
            .AsNoTracking()
            .Where(c =>
                c.Language == languageEnum &&
                c.Article!.CreatedAt >= articleFrom &&
                c.Article.CreatedAt <= asOfUtc &&
                c.Author!.CreatedAt >= userFrom &&
                c.Author.CreatedAt <= asOfUtc)
            .Select(c => new RecentArticleReport(
                c.ArticleId,
                c.Article!.CreatedAt,
                c.Article.Status,
                c.Id,
                c.Title,
                c.Language,
                c.AuthorId,
                c.Author!.Username,
                c.Author.CreatedAt))
            .OrderByDescending(x => x.ArticleCreatedAt)
            .ThenBy(x => x.ArticleId)
            .ToListAsync();
    }
}
