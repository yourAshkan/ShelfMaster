using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelfMaster.Application.Loans.Command;
using ShelfMaster.Application.Loans.Query;
using System.Security.Claims;

namespace ShelfMaster.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoanContoller(IMediator _mediator) : ControllerBase
    {
        #region GetAll
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var loans = await _mediator.Send(new GetAllLoanQuery());
            return Ok(loans);
        }
        #endregion

        #region GetById
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetById(int id)
        {
            var loan = _mediator.Send(new GetLoanByIdQuery(id));
            if (loan == null)
                return NotFound("Loan Not Found!");

            if (User.IsInRole("User"))
            {
                var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (loan.Id == userid)
                    return Forbid("You are not allowed to access this loan!");
            }

            return Ok(loan);
        }
        #endregion

        #region CreateLoan
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([FromBody] CreateLoanCommand command)
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            command.UserId = userid;

            var result = await _mediator.Send(command);
            if (result == null)
                return BadRequest("Loan creation failed!");

            return Ok(result);
        }
        #endregion

        #region UpdateLoan
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLoanCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch!");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        #endregion

        #region DeleteLoan
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteLoanCommand(id));
            if (!result)
                return NotFound("Loan Not Found!");

            return Ok("Loan deleted successfully!");
        } 
        #endregion
    }
}
