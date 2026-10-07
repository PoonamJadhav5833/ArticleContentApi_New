namespace ArticleContentApi.Models;

public class Article
{
    public int Id { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Content> Contents { get; set; } = new List<Content>();
}



    // Get Article details list with content Items   
    [HttpGet]
    public async Task<ActionResult<PagedResponse<ArticleListItem>>> GetArticles(GetArticlesRequest request)
    {
        if (request.pageNumber < 1 || request.pageSize is < 1 or > 100)
            return BadRequest("Invalid pagination values.");

        var result = await _repository.GetArticles(request);

        return Ok(new PagedResponse<ArticleListItem>(
            result.Items,
            request.pageNumber,
            request.pageSize,
            result.TotalCount,
            (int)Math.Ceiling(
                result.TotalCount / (double)request.pageSize)));
    }
