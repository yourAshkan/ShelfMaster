using AutoMapper;
using MediatR;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Application.Interfasces;
using ShelfMaster.Application.Loans.Query;
using ShelfMaster.Domain.Entities;
using ShelfMaster.Domain.Enums;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Loans.Handler;

public class GetAllLoanQueryHandler(ILoanRepository _repo,IMapper _mapper,IUserService _userService) : IRequestHandler<GetAllLoanQuery, List<LoanDto?>>
{
    public async Task<List<LoanDto?>> Handle(GetAllLoanQuery request, CancellationToken cancellationToken)
    {
		try
		{
			var loans = await _repo.GetAllAsync();

			var loanDtos = new List<LoanDto>();

			foreach (var loan in loans)
			{
				var userName = loan.UserId.HasValue
					? await _userService.GetGetUserNameByIdAsync(loan.UserId.Value)
                : "Unknown";

                loanDtos.Add(new LoanDto
                {
                    Id = loan.Id,
                    UserId = loan.UserId,
                    UserEmail = userName,
                    BookId = loan.BookId,
                    BookTitle = loan.Book?.Title,
                    Status = LoanStatus.Approved,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                    IsDeleted = loan.IsDeleted
                });
            }
            return loanDtos;
		}
		catch (Exception ex)
		{
			throw new Exception("Error: ", ex);
		}
    }
}
