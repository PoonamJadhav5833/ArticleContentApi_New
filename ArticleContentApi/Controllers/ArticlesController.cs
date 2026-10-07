using ArticleContentApi.DTOs;
using ArticleContentApi.Models;
using ArticleContentApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ArticleContentApi.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _repository;

    public ArticlesController(IArticleRepository repository) =>
        _repository = repository;

    // Add article with Status and CreatedAt 
    [HttpPost]
    public async Task<IActionResult> Create(CreateArticleRequest request)
    {
        await _repository.AddAsync(request);

        return Ok(new
        {
            message = "Article created successfully."
        });
    }


    // Update Article
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateArticleRequest request)
    {
        var success = await _repository.UpdateAsync(new Article
        {
            Id = id,
            Status = request.Status
        });

        return success ? NoContent() : NotFound();
    }

    
    // get Article by id     
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var article = await _repository.GetByIdAsync(id);

        return article is null
            ? NotFound()
            : Ok(article);
    }

    
    // Delete Article
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        await _repository.DeleteAsync(id) ? NoContent() : NotFound();



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


}
