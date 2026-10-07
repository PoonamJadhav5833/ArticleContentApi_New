using ArticleContentApi.Models;
using ArticleContentApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ArticleContentApi.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportRepository _repository;

    public ReportsController(IReportRepository repository) =>
        _repository = repository;

    [HttpGet("authors")]
    public async Task<IActionResult> Authors()
    {
        var result = await _repository.GetAuthorsWithArticlesAsync();

        return Ok(result);
    }


    [HttpGet("recent-articles")]
    public async Task<IActionResult> RecentArticles([FromQuery] string language,[FromQuery] DateTime? asOfUtc = null)
    {
        var result = await _repository.GetRecentArticlesAsync(
            language,
            asOfUtc ?? DateTime.UtcNow);

        return Ok(result);
    }

}
