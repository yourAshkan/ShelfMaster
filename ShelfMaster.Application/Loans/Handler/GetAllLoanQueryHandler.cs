using AutoMapper;
using MediatR;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Application.Loans.Query;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Loans.Handler;

public class GetAllLoanQueryHandler(ILoanRepository _repo,IMapper _mapper) : IRequestHandler<GetAllLoanQuery, List<LoanDto?>>
{
    public async Task<List<LoanDto?>> Handle(GetAllLoanQuery request, CancellationToken cancellationToken)
    {
		try
		{
			var loans = await _repo.GetAllAsync();
			return _mapper.Map<List<LoanDto?>>(loans);
		}
		catch (Exception ex)
		{
			throw new Exception("Error: ", ex);
		}
    }
}
