using MediatR;
using ShelfMaster.Application.DTOs;

namespace ShelfMaster.Application.Loans.Query;

public class GetLoanByIdQuery(int id) : IRequest<LoanDto?>
{
    public int Id { get; set; } = id;
}
