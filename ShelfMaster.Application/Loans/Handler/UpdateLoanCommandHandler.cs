using AutoMapper;
using MediatR;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Application.Loans.Command;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Loans.Handler;

public class UpdateLoanCommandHandler(ILoanRepository _repo,IMapper _mapper) : IRequestHandler<UpdateLoanCommand, LoanDto>
{
    public async Task<LoanDto> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _repo.GetByIdAsync(request.Id);
            if (loan == null)
                throw new Exception("Loan Not Found!");

            loan.Status = request.Status;
            loan.ReturnDate = request.ReturnDate;

            await _repo.UpdateAsync(loan);
            return _mapper.Map<LoanDto>(loan);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: ", ex);
        }
    }
}
