using MediatR;
using ShelfMaster.Application.DTOs;

namespace ShelfMaster.Application.Loans.Query;

public class GetLoansByUserQuery : IRequest<List<LoanDto?>>
{
    public int? UserId { get; set; }

    public GetLoansByUserQuery(int? userId = null)
    {
        UserId = userId;
    }
}
