using AutoMapper;
using MediatR;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Application.Interfasces;
using ShelfMaster.Application.Loans.Command;
using ShelfMaster.Domain.Entities;
using ShelfMaster.Domain.Enums;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Loans.Handler;

public class CreateLoanCommandHandler(ILoanRepository _repoLoan,IBookRepository _repoBook
    ,IMapper _mapper,IUserService _userService) : IRequestHandler<CreateLoanCommand, LoanDto?>
{
    public async Task<LoanDto?> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var book = await _repoBook.GetByIdAsync(request.BookId ?? 0);

            if (book == null || book.IsDeleted)
                throw new Exception("Book Not Found!");

            if (book.AvailabeCount <= 0)
                throw new Exception("This Book is not Available:(");

            var loan = new Loan
            {
                UserId = request.UserId,
                BookId = request.BookId,
                LoanDate = DateTime.Now,
                Status = LoanStatus.Approved
            };

            await _repoLoan.AddAsync(loan);
            book.AvailabeCount -=1;
            await _repoBook.UpdateAsync(book);

            var userEmail = await _userService.GetGetUserNameByIdAsync(request.UserId ?? 0);
            var bookTitle = book.Title;

            var dto = new LoanDto
            {
                Id = loan.Id,
                UserId = loan.UserId,
                UserEmail = userEmail,
                BookId = loan.BookId,
                BookTitle = bookTitle,
                LoanDate = loan.LoanDate,
                Status = loan.Status,
                ReturnDate = loan.ReturnDate,
            };

            return dto;
        }
        catch (Exception ex)
        {
            throw new Exception("Error: ", ex);
        }
    }
}
