using MediatR;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Domain.Enums;

namespace ShelfMaster.Application.Loans.Command;

public class CreateLoanCommand(int bookid, int userid) : IRequest<LoanDto?>
{
    public int BookId { get; set; } = bookid;
    public int UserId { get; set; } = userid;
    public DateTime LoanDate { get; set; } = DateTime.Now;
    public LoanStatus Status { get; set; } = LoanStatus.Requested;
}
