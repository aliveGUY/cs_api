using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
  [Route("api/commment")]
  [ApiController]
  public class CommentController : ControllerBase
  {
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;
    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
      _commentRepo = commentRepo;
      _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var comment = await _commentRepo.GetAllAsync();
      var commentdto = comment.Select(s => s.ToCommentDto());

      return Ok(commentdto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var comment = await _commentRepo.GetByIdAsync(id);

      if (null == comment)
        return NotFound();

      return Ok(comment.ToCommentDto());
    }

    [HttpPost("{stockid:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      if (!await _stockRepo.StockExists(stockId))
        return BadRequest("Stock does not exist");

      var commentModel = commentDto.ToCommentFromCreate(stockId);
      await _commentRepo.CreateAsync(commentModel);

      return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var comment = await _commentRepo.UpdateAsync(id, commentDto.ToCommentFromUpdate());

      if (null == comment)
        return NotFound("Comment not found");

      return Ok(comment.ToCommentDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var commentModel = await _commentRepo.DeleteAsync(id);

      if (null == commentModel)
        return NotFound("Comment does not exist");

      return Ok(commentModel);
    }
  }
}