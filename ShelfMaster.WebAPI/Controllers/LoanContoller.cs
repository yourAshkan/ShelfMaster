using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelfMaster.Application.Books.Command;
using ShelfMaster.Application.Loans.Command;
using ShelfMaster.Application.Loans.Query;
using System.Security.Claims;

namespace ShelfMaster.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController(IMediator _mediator) : ControllerBase
    {
        #region GetAllLoan
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var loans = await _mediator.Send(new GetAllLoanQuery());
            return Ok(loans);
        }
        #endregion

        #region GetLoanById
        [HttpGet("UserLoan")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetLoansByUser(int? id = null)
        {
            var loans = await _mediator.Send(new GetLoansByUserQuery(id));
            return Ok(loans);
        }
        #endregion

        #region LoanBook
        [HttpPost("LoanBook")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> LoanBook([FromBody] CreateLoanCommand command)
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            command.UserId = userid;

            var result = await _mediator.Send(command);
            if (result == null)
                return BadRequest("Loan creation failed!");

            return Ok(result);
        }
        #endregion

        #region ReturnBook
        [HttpPut("ReturnBook/{loanId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> ReturnBook(int loanId)
        {
            var result = await _mediator.Send(new ReturnBookCommand(loanId));
            return Ok(result);
        }
        #endregion
    }
}
