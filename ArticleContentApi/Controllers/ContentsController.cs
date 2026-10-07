using ArticleContentApi.DTOs;
using ArticleContentApi.Models;
using ArticleContentApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ArticleContentApi.Controllers;

[ApiController]
[Route("api/contents")]
public class ContentsController : ControllerBase
{
    private readonly IContentRepository _repository;

    public ContentsController(IContentRepository repository) =>
        _repository = repository;

    // Add content with article id       
    [HttpPost]
    public async Task<IActionResult> Create(CreateContentRequest request)
    {
        var content = await _repository.AddAsync(request);

        if (content is null)
            return BadRequest("Invalid ArticleId or AuthorId.");

        return Ok(new
        {
            message = "Content created successfully."
        });
    }



   // Update Content with id
   [HttpPut("{id:int}")]
   public async Task<IActionResult> Update(int id, UpdateContentRequest request)
   {
        if (!await _repository.UserExistsAsync(request.AuthorId))
            return BadRequest("AuthorId does not exist.");

        var success = await _repository.UpdateAsync(new Content
        {
            Id = id,
            Title = request.Title,
            ContentText = request.Content,
            AuthorId = request.AuthorId,
            Status = request.Status,
            Language = request.Language
        });

        return success ? NoContent() : NotFound();
    }


    // Delete Content with id
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        await _repository.DeleteAsync(id) ? NoContent() : NotFound();

     
    // get content by id     
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var content = await _repository.GetByIdAsync(id);
        return content is null ? NotFound() : Ok(article);
    }


}
