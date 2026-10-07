using ArticleContentApi.Models;

public interface IContentRepository
{
    Task<Content> AddAsync(Content content);

    Task<Content?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(Content content);

    Task<bool> DeleteAsync(int id);

    Task<bool> ArticleExistsAsync(int articleId);

    Task<bool> UserExistsAsync(int userId);
}
