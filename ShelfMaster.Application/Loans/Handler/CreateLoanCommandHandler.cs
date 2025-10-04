using AutoMapper;
using MediatR;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Application.Loans.Command;
using ShelfMaster.Domain.Entities;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Loans.Handler;

public class CreateLoanCommandHandler(ILoanRepository _repo,IMapper _mapper) : IRequestHandler<CreateLoanCommand, LoanDto?>
{
    public async Task<LoanDto?> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = new Loan
            {
                UserId = request.UserId,
                BookId = request.BookId,
                LoanDate = DateTime.Now,
                Status = request.Status
            };

            await _repo.AddAsync(loan);
            return _mapper.Map<LoanDto>(loan);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: ", ex);
        }
    }
}
