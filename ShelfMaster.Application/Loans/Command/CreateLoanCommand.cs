using MediatR;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Domain.Enums;

namespace ShelfMaster.Application.Loans.Command;

public class CreateLoanCommand : IRequest<LoanDto?>
{
    public int? BookId { get; set; } 
    public int? UserId { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.Now;

    public CreateLoanCommand() 
    {
    }
}
