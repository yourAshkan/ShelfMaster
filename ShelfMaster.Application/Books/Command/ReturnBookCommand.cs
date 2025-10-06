using MediatR;
using ShelfMaster.Application.DTOs;

namespace ShelfMaster.Application.Books.Command;

public class ReturnBookCommand(int loanId) : IRequest<LoanDto?>
{
    public int LoanId { get; set; } = loanId;
}
