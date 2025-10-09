using AutoMapper;
using MediatR;
using ShelfMaster.Application.Books.Command;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Application.Interfasces;
using ShelfMaster.Domain.Enums;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Books.Handler;

public class ReturnBookCommandHandler(IBookRepository _repoBook,ILoanRepository _repoLoan, IMapper _mapper, IUserService _userService) : IRequestHandler<ReturnBookCommand, LoanDto?>
{
    public async Task<LoanDto?> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _repoLoan.GetByIdAsync(request.LoanId);
            if (loan == null)
                throw new Exception("Loan Not Found!");

            if (loan.Status != LoanStatus.Approved)
                throw new Exception("Only approved loans can be returned");

            var book = await _repoBook.GetByIdAsync((int)loan.BookId);
            if (book == null)
                throw new Exception("Book Not Found!");

            loan.Status = LoanStatus.Returned;
            loan.ReturnDate = DateTime.UtcNow;

            await _repoBook.UpdateAsync(book);
            await _repoLoan.UpdateAsync(loan);

            book.AvailableCount++;

            var userEmail = loan.UserId.HasValue ? await _userService.GetGetUserNameByIdAsync(loan.UserId.Value) : null;

            var loanDto = _mapper.Map<LoanDto>(loan);
            loanDto.UserEmail = userEmail;
            loanDto.Status = loan.Status;
            loanDto.BookTitle = book.Title;


            return loanDto;
        }
        catch (Exception ex)
        {
            throw new Exception("Error:", ex);
        }
    }
}
