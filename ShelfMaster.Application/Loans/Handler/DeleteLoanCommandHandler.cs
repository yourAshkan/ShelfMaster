using MediatR;
using ShelfMaster.Application.Loans.Command;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Loans.Handler;

public class DeleteLoanCommandHandler(ILoanRepository _repo) : IRequestHandler<DeleteLoanCommand, bool>
{
    public async Task<bool> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _repo.GetByIdAsync(request.Id);
            if (loan == null)
                throw new Exception("Loan Not Found!");

            loan.SoftDeleted();
            await _repo.UpdateAsync(loan);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error: ", ex);
        }
    }
}
