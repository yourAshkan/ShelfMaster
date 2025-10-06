using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using ShelfMaster.Application.DTOs;
using ShelfMaster.Application.Interfasces;
using ShelfMaster.Application.Loans.Query;
using ShelfMaster.Domain.IRepository;

namespace ShelfMaster.Application.Loans.Handler;

public class GetLoansByUserQueryHandler(ILoanRepository _repo, IUserService _userService,
    IMapper _mapper, IHttpContextAccessor _accessor) : IRequestHandler<GetLoansByUserQuery, List<LoanDto?>>
{
    public async Task<List<LoanDto?>> Handle(GetLoansByUserQuery request, CancellationToken cancellationToken)
    {
        int? userid = request.UserId;
        var user = _accessor.HttpContext!.User;
        if (user.IsInRole("User"))
            userid = int.Parse(user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        else if(user == null)
            throw new Exception("UserId must be provided for Admin");

        var loans = await _repo.GetAllByUserIdAsync(userid.Value);

        var loanDtos = new List<LoanDto>();
        foreach (var loan in loans)
        {
            var dto = _mapper.Map<LoanDto>(loan);
            dto.UserEmail = await _userService.GetGetUserNameByIdAsync(loan.UserId!.Value);
            dto.Status = loan.Status;
            loanDtos.Add(dto);
        }
        return loanDtos;
    }
}
