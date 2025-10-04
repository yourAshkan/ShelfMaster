using MediatR;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Domain.Enums;

namespace ShelfMaster.Application.Loans.Command;

public class UpdateLoanCommand(int id, int bookid, int userid, DateTime loandate, DateTime? returndate,LoanStatus status) : IRequest<LoanDto>
{
    public int Id { get; set; } = id;
    public int BookId { get; set; } = bookid;
    public int UserId { get; set; } = userid;
    public DateTime LoanDate { get; set; } = loandate;
    public DateTime? ReturnDate { get; set; } = returndate;
    public LoanStatus Status { get; set; } = status;
}
