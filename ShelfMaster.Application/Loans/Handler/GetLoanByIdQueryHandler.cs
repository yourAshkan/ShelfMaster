using AutoMapper;
using MediatR;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Application.Loans.Query;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Loans.Handler;

public class GetLoanByIdQueryHandler(ILoanRepository _repo,IMapper _mapper) : IRequestHandler<GetLoanByIdQuery, LoanDto?>
{
    public async Task<LoanDto?> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
		try
		{
			var loan = await _repo.GetByIdAsync(request.Id);

			return _mapper.Map<LoanDto>(loan);
		}
		catch (Exception ex)
		{
			throw new Exception("Error: ", ex);
		}
    }
}
