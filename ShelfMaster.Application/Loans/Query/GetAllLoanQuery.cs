using MediatR;
using ShelfMaster.Application.DTOs;

namespace ShelfMaster.Application.Loans.Query;

public class GetAllLoanQuery : IRequest<List<LoanDto?>>
{
}
