using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelfMaster.Application.Books.Command;
using ShelfMaster.Application.Books.Query;

namespace ShelfMaster.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookContoller(IMediator _mediator) : ControllerBase
    {
        #region GetAll
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var books = await _mediator.Send(new GetAllBookQuery());
            return Ok(books);
        }
        #endregion

        #region GetById
        [HttpGet("ById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));
            if (book == null)
                return NotFound();

            return Ok(book);
        }
        #endregion

        #region CreateBook
        [HttpPost("CreateBook")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateBookCommand command)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            command.SetUserId(int.Parse(userIdClaim));

            var book = await _mediator.Send(command);
            return Ok(book);
        }
        #endregion

        #region UpdateBook
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateBookCommand command)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            command.Id = id;

            var book = await _mediator.Send(command);
            if (book == null)
                return NotFound();

            return Ok("Book Updated.");
        }
        #endregion

        #region DeleteBook
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("User ID Not Found!");

            var currentUserId = int.Parse(userIdClaim);

            var result = await _mediator.Send(new DeleteBookCommand(currentUserId));

            if (!result)
                return BadRequest("You dont have permission to access!");

            return Ok("Book Deleted!");
        } 
        #endregion
    }
}
