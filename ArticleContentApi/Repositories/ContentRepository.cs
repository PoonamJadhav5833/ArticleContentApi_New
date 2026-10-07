using ArticleContentApi.Data;
using ArticleContentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleContentApi.Repositories;

public class ContentRepository : IContentRepository
{
    private readonly AppDbContext _db;

    public ContentRepository(AppDbContext db) => _db = db;

    public async Task<Content?> AddAsync(CreateContentRequest request)
    {
        if (!await ArticleExistsAsync(request.ArticleId))
            return null;

        if (!await UserExistsAsync(request.AuthorId))
            return null;

        var content = new Content
        {
            Title = request.Title,
            ContentText = request.Content,
            AuthorId = request.AuthorId,
            Status = request.Status,
            Language = request.Language,
            ArticleId = request.ArticleId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Contents.Add(content);
        await _db.SaveChangesAsync();

        await _db.Entry(content)
            .Reference(x => x.Author)
            .LoadAsync();

        return content;
    }


    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _db.Contents
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }


    public async Task<bool> UpdateAsync(Content content)
    {
        var existing = await _db.Contents.FindAsync(content.Id);
        if (existing is null) return false;

        existing.Title = content.Title;
        existing.ContentText = content.ContentText;
        existing.AuthorId = content.AuthorId;
        existing.Status = content.Status;
        existing.Language = content.Language;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var content = await _db.Contents.FindAsync(id);
        if (content is null) return false;

        _db.Contents.Remove(content);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<bool> ArticleExistsAsync(int articleId) =>
        _db.Articles.AnyAsync(a => a.Id == articleId);

    public Task<bool> UserExistsAsync(int userId) =>
        _db.Users.AnyAsync(u => u.Id == userId);
}
